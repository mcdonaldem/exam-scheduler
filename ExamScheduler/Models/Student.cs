namespace ExamScheduler.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentExamDetail> ExamDetails { get; set; }

        public Student()
        {
            Enrollments = new List<Enrollment>();
            ExamDetails = new List<StudentExamDetail>();
        }
    }
}
