using ExamScheduler.Contexts;
using ExamScheduler.Services;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Tests.TestSetup;
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

        public async Task CreateSchedule_ValidData_ReturnsOK()
        {

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
    }
}