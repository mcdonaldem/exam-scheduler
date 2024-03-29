using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Entities;

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

        public static ApplicationContext SeedCourses(ApplicationContext context)
        {
            List<Course> coursesToSave = [
                new Course("Winter 2023"),
                new Course("Spring 2024")
            ];

            context.Courses.AddRange(coursesToSave);
            context.SaveChanges();
            return context;
        }

        public static ApplicationContext SeedStudents(ApplicationContext context)
        {
            List<Student> studentsToSave = [
                new Student("Noah Nordstrom"),
                new Student("Albin Latos"),
                new Student("Gloriana Parrilla"),
                new Student("Odin Pageau"),
                new Student("Lukas Kellenberger"),
                new Student("Eliyahu Mor"),
                new Student("Gregor Dambach"),
                new Student("Bernhardt Glawe"),
                new Student("Treasa Dooney"),
                new Student("Georgina Kazmer"),
                new Student("Donalda McCrimmon"),
                new Student("Micaela Santoro"),
                new Student("Jantzen Blitz"),
                new Student("Guillermo Navarro"),
                new Student("Canon Stovall"),
                new Student("Krzysztof Krysinski"),
                new Student("Kaden Mace"),
                new Student("Callum Matteson"),
                new Student("Remi Schurr"),
                new Student("Omer Kinderknecht"),
                new Student("Haya Komar"),
                new Student("Owen Gettings"),
                new Student("Boden Menninger"),
                new Student("Placido Pelino"),
                new Student("Adham Byker")
            ];

            context.Students.AddRange(studentsToSave);
            context.SaveChanges();
            return context;
        }

        public static ApplicationContext SeedEnrollments(ApplicationContext context)
        {
            var courses = context.Courses
                .ToArray()
                ;

            var students = context.Students
                .ToArray()
                ;

            List<Enrollment> enrollments = [];
            foreach (var s in students)
            {
                enrollments.Add(new Enrollment
                {
                    Student = s,
                    Course = courses[new Random().Next(courses.Length)]
                });
            }
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
            return context;
        }
    }
}
