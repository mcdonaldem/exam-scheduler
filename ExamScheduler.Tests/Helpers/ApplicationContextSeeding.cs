using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Entities;

namespace ExamScheduler.Tests.Helpers
{
    internal class ApplicationContextSeeding
    {
        public static ApplicationContext SeedAlgoLanguages(ApplicationContext context)
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

        public static ApplicationContext SeedMentors(ApplicationContext context)
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
