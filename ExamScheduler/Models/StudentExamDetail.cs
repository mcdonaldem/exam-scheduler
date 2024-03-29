namespace ExamScheduler.Entities
{
    public class StudentExamDetail
    {
        public Student Student { get; set; }
        public AlgoLanguage AlgoLanguage { get; set; }

        public override string ToString()
        {
            return $"{Student.Name};{AlgoLanguage.Name}";
        }
    }
}
