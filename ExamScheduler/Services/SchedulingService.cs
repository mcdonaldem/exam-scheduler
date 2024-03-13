using ExamScheduler.Contexts;
using ExamScheduler.Models.Enums;

namespace ExamScheduler.Services
{
    public class SchedulingService
    {
        private ApplicationContext _context;
        private ParsingService _parsingService;

        public SchedulingService(ApplicationContext context, ParsingService parsubgService)
        {
            _context = context;
            _parsingService = parsubgService;
        }

        public TimeOnly GetStartTime(TimeSlot timeSlot) => timeSlot switch
        {
            TimeSlot.Morning => TimeOnly.Parse(Environment.GetEnvironmentVariable("MORNING_START")),
            TimeSlot.EarlyAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("EARLY_AFTERNOON_START")),
            TimeSlot.LateAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("LATE_AFTERNOON_START"))
        };
    }
}
