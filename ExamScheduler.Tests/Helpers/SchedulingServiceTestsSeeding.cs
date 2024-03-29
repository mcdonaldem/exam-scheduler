using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using Microsoft.AspNetCore.Http;

namespace ExamScheduler.Tests.Helpers
{
    public class SchedulingServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            MentorServiceTestsSeeding.Seed(context);
            StudentServiceTestsSeeding.Seed(context);
            return context;
        }
    }
}
