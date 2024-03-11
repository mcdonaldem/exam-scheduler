using ExamScheduler.Models.Enums;

namespace ExamScheduler.Models
{
    public class StudentExamDetail
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Exam Exam { get; set; }
        public AlgoLanguage AlgoLanguage { get; set; }
    }
}
