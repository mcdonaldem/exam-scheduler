using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using ExamScheduler.Models;
using ExamScheduler.Services;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Tests.TestSetup;
using Microsoft.AspNetCore.Http;
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CreateSchedule_ValidInput_ReturnsOK(int courseId)
        {
            // Arrange
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

            // Act
            var result = service.CreateSchedule(mentorFile, studentFile, courseId);

            // Assert
            Assert.Equal(studentDetails.Count, result.Count);
        }

        public async Task CreateSchedule_InsufficientMentorSlotsOverall_ThrowsSchedulingException()
        {

        }

        public async Task CreateSchedule_InsufficientMentorSlotsForLang_ThrowsSchedulingException()
        {

        }

        public async Task CreateSchedule_DateTimeParseFail_ThrowsSchedulingException()
        {

        }

        public async Task CreateSchedule_TimeSpanParseFail_ThrowsSchedulingException()
        {

        }

        private void MentorAvailsSetup(int courseId)
        {
            mentorAvails = GetMentorAvailabilities(context, courseId);
        }

        private void StudentDetailsSetup(int courseId)
        {
            studentDetails = GetStudentExamDetails(context, courseId);
        }
    }
}