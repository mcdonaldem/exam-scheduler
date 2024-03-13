namespace ExamScheduler.Entities
{
    public class Student : Person
    {
        public List<Enrollment> Enrollments { get; set; }
        public List<Exam> Exams { get; set; }

        public Student()
        {
            Enrollments = [];
            Exams = [];
        }
    }
}
