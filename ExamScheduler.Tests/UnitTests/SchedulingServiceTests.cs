using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using ExamScheduler.Exceptions;
using ExamScheduler.Models;
using ExamScheduler.Models.Enums;
using ExamScheduler.Services;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Tests.TestSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using static ExamScheduler.Tests.Helpers.SchedulingServiceTestsConfig;
using static ExamScheduler.Tests.Helpers.SchedulingServiceTestsSeeding;

namespace ExamScheduler.Tests.Services
{
    public class SchedulingServiceTests : TestsBase
    {
        private ApplicationContext context;
        private Mock<IParsingService> parsingService;
        private IConfiguration configuration;
        private SchedulingService service;
        private List<MentorAvailability> mentorAvails;
        private List<StudentExamDetail> studentDetails;

        public SchedulingServiceTests()
        {
            context = GetSqliteDbContext();
            Seed(context);
            parsingService = new Mock<IParsingService>();
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(GetConfigurationKVPs())
                .Build()
                ;
            service = new SchedulingService(context, parsingService.Object, configuration);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task CreateSchedule_ValidInput_ReturnsOK(int courseId)
        {
            // Arrange
            #region
            MentorAvailsSetup(courseId);
            StudentDetailsSetup(courseId);

            var mentorFile = ConvertToFormFile(mentorAvails);
            var studentFile = ConvertToFormFile(studentDetails);

            parsingService
                .Setup(ps => ps.GetStudentExamDetails(studentFile, courseId))
                .Returns(studentDetails)
                ;

            parsingService
                .Setup(ps => ps.GetMentorAvailabilities(mentorFile))
                .Returns(mentorAvails)
                ;
            #endregion

            // Act
            var result = service.CreateSchedule(mentorFile, studentFile, courseId);

            // Assert
            Assert.Equal(studentDetails.Count, result.Count);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task CreateSchedule_InsufficientMentorSlotsOverall_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            #region
            MentorAvailsSetup(courseId);
            StudentDetailsSetup(courseId);

            mentorAvails = mentorAvails
                .Take(studentDetails.Count - 1)
                .ToList()
                ;

            var mentorFile = ConvertToFormFile(mentorAvails);
            var studentFile = ConvertToFormFile(studentDetails);

            parsingService
                .Setup(ps => ps.GetStudentExamDetails(studentFile, courseId))
                .Returns(studentDetails)
                ;

            parsingService
                .Setup(ps => ps.GetMentorAvailabilities(mentorFile))
                .Returns(mentorAvails)
                ;
            #endregion

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.CreateSchedule(mentorFile, studentFile, courseId));

            // Assert
            Assert.Equal("Not enough exam slots to cover all students.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task CreateSchedule_InsufficientMentorSlotsForLang_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            #region
            MentorAvailsSetup(courseId);
            StudentDetailsSetup(courseId);

            var examsPerLang = studentDetails
                .GroupBy(s => s.AlgoLanguage)
                .ToDictionary(s => s.Key, s => s.Count())
                ;

            var maxSlotsPerLang = studentDetails
                .Select(s => s.AlgoLanguage)
                .Distinct()
                .ToDictionary(s => s, s => mentorAvails
                    .Where(m => m.Mentor.AlgoLanguages.Contains(s))
                    .ToList()
                    )
                ;

            var mostPopLang = examsPerLang
                .MaxBy(l => l.Value)
                ;

            var slotsToRemove = maxSlotsPerLang[mostPopLang.Key]
                .Take(maxSlotsPerLang[mostPopLang.Key].Count - mostPopLang.Value + 1)
                ;

            mentorAvails.RemoveAll(ma => slotsToRemove.Contains(ma));

            var mentorFile = ConvertToFormFile(mentorAvails);
            var studentFile = ConvertToFormFile(studentDetails);

            parsingService
                .Setup(ps => ps.GetStudentExamDetails(studentFile, courseId))
                .Returns(studentDetails)
                ;

            parsingService
                .Setup(ps => ps.GetMentorAvailabilities(mentorFile))
                .Returns(mentorAvails)
                ;
            #endregion

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.CreateSchedule(mentorFile, studentFile, courseId));

            // Assert
            Assert.Contains("Not enough mentor availability to cover", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task CreateSchedule_DateTimeParseFail_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            #region
            MentorAvailsSetup(courseId);
            StudentDetailsSetup(courseId);

            mentorAvails
                .ForEach(ma => ma.TimeSlot = (TimeSlot)4)
                ;

            var mentorFile = ConvertToFormFile(mentorAvails);
            var studentFile = ConvertToFormFile(studentDetails);

            parsingService
                .Setup(ps => ps.GetStudentExamDetails(studentFile, courseId))
                .Returns(studentDetails)
                ;

            parsingService
                .Setup(ps => ps.GetMentorAvailabilities(mentorFile))
                .Returns(mentorAvails)
                ;
            #endregion

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.CreateSchedule(mentorFile, studentFile, courseId));

            // Assert
            Assert.Contains("No such time slot found.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task CreateSchedule_TimeSpanParseFail_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            #region
            configuration["ExamDuration"] = string.Empty;

            MentorAvailsSetup(courseId);
            StudentDetailsSetup(courseId);

            var mentorFile = ConvertToFormFile(mentorAvails);
            var studentFile = ConvertToFormFile(studentDetails);

            parsingService
                .Setup(ps => ps.GetStudentExamDetails(studentFile, courseId))
                .Returns(studentDetails)
                ;

            parsingService
                .Setup(ps => ps.GetMentorAvailabilities(mentorFile))
                .Returns(mentorAvails)
                ;
            #endregion

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.CreateSchedule(mentorFile, studentFile, courseId));

            // Assert
            Assert.Equal(typeof(FormatException), result.InnerException?.GetType());

        }

        private void MentorAvailsSetup(int courseId)
        {
            mentorAvails = GetMentorAvailabilities(context, courseId);
        }

        private void StudentDetailsSetup(int courseId)
        {
            studentDetails = GetStudentExamDetails(context, courseId);
        }

        public static IEnumerable<object[]> GetAlgoLanguages()
        {
            yield return [new AlgoLanguage("Java")];
            yield return [new AlgoLanguage("C#")];
            yield return [new AlgoLanguage("TypeScript")];
            yield return [new AlgoLanguage("Python")];
        }

        public static IEnumerable<object[]> GetValidCourseIds()
        {
            for (int i = 1; i < 3; i++)
            {
                yield return [i];
            }
        }
    }
}