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
            AlgoLanguages = [];
        }

        public Mentor(string name) : base(name)
        {
            IsActive = true;
            Courses = [];
            Exams = [];
            AlgoLanguages = [];
        }

        public Mentor(string name, bool isActive) : base(name)
        {
            IsActive = isActive;
            Courses = [];
            Exams = [];
            AlgoLanguages = [];
        }
    }
}
