using ExamScheduler.Contexts;
using ExamScheduler.Services;
using ExamScheduler.Tests.TestSetup;
using static ExamScheduler.Tests.Helpers.MentorServiceTestsSeeding;

namespace ExamScheduler.Tests.Services
{
    public class MentorServiceTests : TestsBase
    {
        private ApplicationContext context;
        private MentorService service;

        public MentorServiceTests()
        {
            context = GetSqliteDbContext();
            Seed(context);
            service = new MentorService(context);
        }

        [Fact]
        public async Task GetAllActive_PopulatedDb_ReturnsOK()
        {
            // Act
            var mentors = service.GetAllActive();

            // Assert
            Assert.DoesNotContain(mentors, m => !m.IsActive);
            Assert.NotEmpty(mentors.SelectMany(m => m.AlgoLanguages));
        }

        [Fact]
        public async Task GetAllActive_EmptyDb_ReturnsOK()
        {
            // Arrange
            EmptyDb();

            // Act
            var mentors = service.GetAllActive();

            // Assert
            Assert.Empty(mentors);
        }


        [Fact]
        public async Task GetActiveAlgoLanguages_PopulatedDb_ReturnsOK()
        {
            // Act
            var algoLangs = service.GetActiveAlgoLanguages();

            // Assert
            Assert.NotEmpty(algoLangs);
        }

        [Fact]
        public async Task GetActiveAlgoLanguages_EmptyDb_ReturnsOK()
        {
            // Arrange
            EmptyDb();

            // Act
            var algoLangs = service.GetActiveAlgoLanguages();

            // Assert
            Assert.Empty(algoLangs);
        }

        private void EmptyDb()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}