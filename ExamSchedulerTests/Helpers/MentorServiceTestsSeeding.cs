using ExamScheduler.Contexts;
using ExamScheduler.Entities;

namespace ExamSchedulerTests.Helpers
{
    public static class MentorServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            SeedAlgoLanguages(context);
            SeedMentors(context);
            return context;
        }

        private static ApplicationContext SeedAlgoLanguages(ApplicationContext context)
        {
            // Seed algo languages
            List<AlgoLanguage> algoLangs = [
                new AlgoLanguage("Java"),
                new AlgoLanguage("Python"),
                new AlgoLanguage("TypeScript"),
                new AlgoLanguage("C#")
            ];

            context.AlgoLanguages
                .AddRange(algoLangs)
                ;
            context.SaveChanges();
            return context;
        }

        private static ApplicationContext SeedMentors(ApplicationContext context)
        {
            var langs = context.AlgoLanguages
                .ToList()
                ;

            // Seed Mentors
            List<Mentor> mentors = [
                new Mentor("Arthur", true),
                new Mentor("Beatrice", true),
                new Mentor("Calvin", false),
                new Mentor("Delia", true),
                new Mentor("Ethan", true),
                new Mentor("Fiona", false),
                new Mentor("George", true),
                new Mentor("Hanna", true)
            ];

            foreach (var m in mentors)
            {
                var knownLangs = new Random().Next(1, langs.Count + 1);
                var indices = Enumerable
                    .Range(0, langs.Count)
                    .ToList()
                    ;
                for (int i = 1; i <= knownLangs; i++)
                {
                    var randIndex = new Random().Next(indices.Count());
                    m.AlgoLanguages.Add(langs[indices[randIndex]]);
                    indices.RemoveAt(randIndex);
                }
            }

            context.Mentors.AddRange(mentors);
            context.SaveChanges();
            return context;
        }
    }
}
