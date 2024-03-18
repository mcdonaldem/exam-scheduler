using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Services;
using ExamSchedulerTests.Helpers;
using ExamSchedulerTests.TestSetup;
using Microsoft.EntityFrameworkCore;

namespace ExamSchedulerTests.Services
{
    public class StudentServiceTests : TestsBase
    {
        private ApplicationContext context;
        private StudentService service;

        public StudentServiceTests()
        {
            context = GetSqliteDbContext();
            StudentServiceTestsSeeding.Seed(context);
            service = new StudentService(context);
        }

        [Theory]
        [InlineData(IdValue.Max)]
        [InlineData(IdValue.Min)]
        [InlineData((IdValue)4)]
        public async Task GetAllByCourse_ValidId_PopulatedDb_ReturnsOK(IdValue courseIdSelect)
        {
            // Arrange
            var courseId = courseIdSelect switch
            {
                IdValue.Max => context.Courses.Select(c => c.Id).Max(),
                IdValue.Min => context.Courses.Select(c => c.Id).Min(),
                _ => context.Courses.Select(c => c.Id).FirstOrDefault()
            };

            // Act
            var students = service.GetAllByCourse(courseId);

            // Assert
            Assert.NotEmpty(students);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public async Task GetAllByCourse_InvalidId_PopulatedDb_ThrowsException(int courseId)
        {
            // Act
            var result = Assert.Throws<ArgumentException>(() => service.GetAllByCourse(courseId));

            // Assert
            Assert.Equal("Invalid course id.", result.Message);
        }

        [Fact]
        public async Task GetAllByCourse_ValidId_EmptyStudentsTable_ReturnsOK()
        {
            // Arrange
            RemoveStudents();
            var courseId = context.Courses
                .Select(c => c.Id)
                .Min()
                ;

            // Act
            var students = service.GetAllByCourse(courseId);

            // Assert
            Assert.Empty(students);
        }

        private void RemoveStudents()
        {
            context.Students.ExecuteDelete();
            context.SaveChanges();
        }

        public enum IdValue
        {
            Max,
            Min
        }
    }
}
