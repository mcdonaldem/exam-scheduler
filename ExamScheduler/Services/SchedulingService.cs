using ExamScheduler.Models.Enums;

namespace ExamScheduler.Services
{
    public class SchedulingService
    {
        public TimeOnly GetStartTime(TimeSlot timeSlot) => timeSlot switch
        {
            TimeSlot.Morning => TimeOnly.Parse(Environment.GetEnvironmentVariable("MORNING_START")),
            TimeSlot.EarlyAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("EARLY_AFTERNOON_START")),
            TimeSlot.LateAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("LATE_AFTERNOON_START"))
        };
    }
}
