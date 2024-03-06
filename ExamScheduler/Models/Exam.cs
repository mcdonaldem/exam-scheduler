namespace ExamScheduler.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public List<Mentor> Mentors { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
