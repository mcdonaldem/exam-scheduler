﻿using ExamScheduler.Contexts;
using ExamScheduler.Services;
using ExamScheduler.Tests.TestSetup;
using Microsoft.EntityFrameworkCore;
using static ExamScheduler.Tests.Helpers.StudentServiceTestsSeeding;

namespace ExamScheduler.Tests.Services
{
    public class StudentServiceTests : TestsBase
    {
        private ApplicationContext context;
        private StudentService service;

        public StudentServiceTests()
        {
            context = GetSqliteDbContext();
            Seed(context);
            service = new StudentService(context);
        }

        [Theory]
        [InlineData(IdValue.Max)]
        [InlineData(IdValue.Min)]
        [InlineData((IdValue)4)]
        public async Task GetAllByCourse_ValidId_PopulatedDb_ReturnsOK(IdValue id)
        {
            // Arrange
            var courseId = GetCourseId(id);

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

        [Theory]
        [InlineData(IdValue.Max)]
        [InlineData(IdValue.Min)]
        [InlineData((IdValue)4)]
        public async Task GetAllByCourse_ValidId_EmptyStudentsTable_ReturnsOK(IdValue id)
        {
            // Arrange
            RemoveStudents();
            var courseId = GetCourseId(id);

            // Act
            var students = service.GetAllByCourse(courseId);

            // Assert
            Assert.Empty(students);
        }

        private int GetCourseId(IdValue courseId) => courseId switch
        {
            IdValue.Max => context.Courses.Select(c => c.Id).Max(),
            IdValue.Min => context.Courses.Select(c => c.Id).Min(),
            _ => context.Courses.Select(c => c.Id).FirstOrDefault()
        };

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
