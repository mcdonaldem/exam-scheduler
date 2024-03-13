namespace ExamScheduler.Entities
{
    public class Mentor : Person
    {
        public bool IsActive { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }
        public List<AlgoLanguage> AlgoLanguages { get; set; }

        public Mentor()
        {
            Courses = [];
            Exams = [];
        }
    }
}
