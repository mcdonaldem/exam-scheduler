namespace ExamScheduler.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Mentor Mentor { get; set; }
        public AlgoLanguage AlgoLanguage { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string ToFileString()
        {
            return $"{Student?.Name};{Mentor?.Name};{AlgoLanguage?.Name};{Start};{End}";
        }
    }
}
