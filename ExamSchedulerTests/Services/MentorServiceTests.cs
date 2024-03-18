using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Services;
using ExamScheduler.Services.Interfaces;
using ExamSchedulerTests.Helpers;
using ExamSchedulerTests.TestSetup;

namespace ExamSchedulerTests.Services
{
    public class MentorServiceTests : TestsBase
    {
        private ApplicationContext context;
        private MentorService service;

        public MentorServiceTests()
        {
            context = GetSqliteDbContext();
            MentorTestsSeeding.Seed(context);
            service = new MentorService(context);
        }

        [Fact]
        public async Task GetAllActive_ReturnsOK()
        {
            // Act
            var mentors = service.GetAllActive();

            // Assert
            Assert.DoesNotContain(mentors, m => !m.IsActive);
            Assert.NotEmpty(mentors.SelectMany(m => m.AlgoLanguages));
        }
    }
}
