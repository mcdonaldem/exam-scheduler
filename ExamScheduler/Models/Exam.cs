using ExamScheduler.Models.Enums;

namespace ExamScheduler.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public StudentExamDetail StudentDetail { get; set; }
        public List<Mentor> Mentors { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Exam()
        {
            Mentors = new List<Mentor>();
        }
    }
}
