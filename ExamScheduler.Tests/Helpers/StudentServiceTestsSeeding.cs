using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using static ExamScheduler.Tests.Helpers.ApplicationContextSeeding;

namespace ExamScheduler.Tests.Helpers
{
    public class StudentServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            SeedCourses(context);
            SeedStudents(context);
            SeedEnrollments(context);
            return context;
        }
    }
}
