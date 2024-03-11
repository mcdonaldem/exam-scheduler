namespace ExamScheduler.Models
{
    public class Mentor : Person
    {
        public bool IsActive { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }

        public Mentor()
        {
            Courses = new List<Course>();
            Exams = new List<Exam>();
        }
    }
}
