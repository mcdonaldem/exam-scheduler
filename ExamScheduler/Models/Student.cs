namespace ExamScheduler.Models
{
    public class Student : Person
    {
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentExamDetail> ExamDetails { get; set; }

        public Student()
        {
            Enrollments = new List<Enrollment>();
            ExamDetails = new List<StudentExamDetail>();
        }
    }
}
