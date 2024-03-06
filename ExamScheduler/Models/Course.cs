namespace ExamScheduler.Models
{
    public class Course
    {
        public int Id { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Mentor> Mentors { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
    }
}
