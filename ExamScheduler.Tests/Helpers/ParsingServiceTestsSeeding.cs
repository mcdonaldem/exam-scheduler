using ExamScheduler.Contexts;
using static ExamScheduler.Tests.Helpers.ApplicationContextSeeding;


namespace ExamScheduler.Tests.Helpers
{
    public class ParsingServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            SeedAlgoLanguages(context);
            SeedMentors(context);
            SeedCourses(context);
            SeedStudents(context);
            SeedEnrollments(context);
            return context;
        }
    }
}
