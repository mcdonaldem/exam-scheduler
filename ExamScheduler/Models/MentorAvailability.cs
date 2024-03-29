using ExamScheduler.Entities;
using ExamScheduler.Models.Enums;

namespace ExamScheduler.Models
{
    public class MentorAvailability
    {
        public Mentor Mentor { get; set; }
        public DateOnly Date { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public override string ToString()
        {
            return $"{Mentor?.Name};{Date.ToShortDateString()};{TimeSlot}";
        }
    }
}
