namespace ExamScheduler.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public int StudentDetailId { get; set; }
        public Student Student { get; set; }
        public List<Mentor> Mentors { get; set; }
        public AlgoLanguage AlgoLanguage { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Exam()
        {
            Mentors = new List<Mentor>();
        }
    }
}
