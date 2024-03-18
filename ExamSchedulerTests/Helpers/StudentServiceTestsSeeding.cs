using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Entities;

namespace ExamSchedulerTests.Helpers
{
    public class StudentServiceTestsSeeding
    {
        public static ApplicationContext Seed(ApplicationContext context)
        {
            List<Course> coursesToSave = [
                new Course("Winter 2023"),
                new Course("Spring 2024"),
                new Course("Fall 2024"),
                new Course("Winter 2024")
            ];

            context.Courses.AddRange(coursesToSave);

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

            var courses = context.Courses
                .ToList()
                ;

            var students = context.Students
                .ToList()
                ;

            List<Enrollment> enrollments = [];

            foreach (var s in students)
            {
                enrollments.Add(new Enrollment
                {
                    Student = s,
                    Course = courses[new Random().Next(courses.Count)]
                });
            }

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
            return context;
        }
    }
}
