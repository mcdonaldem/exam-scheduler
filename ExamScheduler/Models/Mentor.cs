namespace ExamScheduler.Models
{
    public class Mentor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }
    }
}
