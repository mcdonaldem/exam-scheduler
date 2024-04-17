using ExamScheduler.Contexts;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Services;
using Moq;
using ExamScheduler.Tests.TestSetup;
using static ExamScheduler.Tests.Helpers.ParsingServiceTestsSeeding;
using static ExamScheduler.Tests.Helpers.ParsingServiceTestsConfig;
using ExamScheduler.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace ExamScheduler.Tests.Services
{
    public class ParsingServiceTests : TestsBase
    {
        private ApplicationContext context;
        private Mock<IMentorService> mentorService;
        private Mock<IStudentService> studentService;
        private ParsingService service;

        public ParsingServiceTests()
        {
            context = GetSqliteDbContext();
            Seed(context);
            mentorService = new Mock<IMentorService>();
            studentService = new Mock<IStudentService>();
            service = new ParsingService(mentorService.Object, studentService.Object);
        }

        #region
        [Fact]
        public async Task GetMentorAvailabilities_NullInput_ThrowsSchedulingException()
        {
            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetMentorAvailabilities(null));

            // Assert
            Assert.Equal("No file provided.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_WrongFileContentType_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            var input = (FormFile)ConvertToFormFile(GetMentorAvailabilities(context,courseId));
            input.ContentType = "csv";

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Equal("Incorrect file type provided.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_EmptyFile_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            var input = (FormFile)ConvertToFormFile(new List<string>());

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Equal("Provided file is empty.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_InvalidMentorNames_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            mentorService.Setup(m => m.GetAllActive()).Returns(context.Mentors.Where(m => m.IsActive).ToList());
            context.ChangeTracker.Clear();
            var input = ConvertToFormFile(InsertIncorrectMentorNames(GetMentorAvailabilities(context, courseId)));

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Contains("with name", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_IncorrectColumnQuantity_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            var input = ConvertToFormFile(GetMentorAvailabilities(context, courseId).Select(ma => ma.Mentor).ToList());

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Contains("columns instead", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_IncorrectDateFormat_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            mentorService
                .Setup(m => m.GetAllActive())
                .Returns(context.Mentors.Where(m => m.IsActive).ToList())
                ;
            var input = ConvertToFormFile(InsertIncorrectDateFormat(GetMentorAvailabilities(context, courseId)));

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Contains("Invalid date or time given.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetMentorAvailabilities_IncorrectTimeSlot_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            mentorService
                .Setup(m => m.GetAllActive())
                .Returns(context.Mentors.Where(m => m.IsActive).ToList())
                ;
            var input = ConvertToFormFile(InsertIncorrectTimeSlots(GetMentorAvailabilities(context, courseId)));

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Contains("Invalid date or time given.", result.Message);
        }
        #endregion

        #region
        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_NullInput_ThrowsSchedulingException(int courseId)
        {
            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetStudentExamDetails(null, courseId));

            // Assert
            Assert.Equal("No file provided.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_WrongFileContentType_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            var input = (FormFile)ConvertToFormFile(GetStudentExamDetails(context, courseId));
            input.ContentType = "csv";

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetStudentExamDetails(input, courseId));

            // Assert
            Assert.Equal("Incorrect file type provided.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_EmptyFile_ThrowsSchedulingException(int courseId)
        {
            // Arrange
            var input = (FormFile)ConvertToFormFile(new List<string>());

            // Act
            var result = Assert.Throws<SchedulingException>(() => service.GetStudentExamDetails(input, courseId));

            // Assert
            Assert.Equal("Provided file is empty.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_InvalidStudentNames_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            studentService
                .Setup(s => s.GetAllByCourse(courseId))
                .Returns(context
                    .Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.Student)
                    .ToList())
                ;
            context.ChangeTracker.Clear();
            var input = ConvertToFormFile(InsertIncorrectStudentNames(GetStudentExamDetails(context, courseId)));

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetStudentExamDetails(input, courseId));

            // Assert
            Assert.Contains("with name", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_InvalidAlgo_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            studentService
                .Setup(s => s.GetAllByCourse(courseId))
                .Returns(context
                    .Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.Student)
                    .ToList())
                ;
            context.ChangeTracker.Clear();
            var input = ConvertToFormFile(InsertIncorrectAlgos(GetStudentExamDetails(context, courseId)));

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetStudentExamDetails(input, courseId));

            // Assert
            Assert.Equal("Invalid algo language.", result.Message);
        }

        [Theory]
        [MemberData(nameof(GetValidCourseIds))]
        public async Task GetStudentExamDetails_IncorrectColumnQuantity_ThrowsInvalidFileDataException(int courseId)
        {
            // Arrange
            var input = ConvertToFormFile(GetStudentExamDetails(context, courseId).Select(d => d.Student).ToList());

            // Act
            var result = Assert.Throws<InvalidFileDataException>(() => service.GetMentorAvailabilities(input));

            // Assert
            Assert.Contains("columns instead", result.Message);
        }
        #endregion

        public static IEnumerable<object[]> GetValidCourseIds()
        {
            for (int i = 1; i < 3; i++)
            {
                yield return [i];
            }
        }
    }
}
