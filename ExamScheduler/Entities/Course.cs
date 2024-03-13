namespace ExamScheduler.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Mentor> Mentors { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }

        public Course()
        {
            Enrollments = new List<Enrollment>();
            Mentors = new List<Mentor>();
        }
    }
}
