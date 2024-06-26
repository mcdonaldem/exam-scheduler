﻿using ExamScheduler.Contexts;
using static ExamScheduler.Tests.Helpers.ApplicationContextSeeding;

namespace ExamScheduler.Tests.Helpers
{
    public static class MentorServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            SeedAlgoLanguages(context);
            SeedMentors(context);
            return context;
        }
    }
}
