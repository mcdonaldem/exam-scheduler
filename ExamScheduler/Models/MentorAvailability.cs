using ExamScheduler.Entities;
using ExamScheduler.Entities.Enums;

namespace ExamScheduler.Models
{
    public class MentorAvailability
    {
        public Mentor Mentor { get; set; }
        public DateOnly Date { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
