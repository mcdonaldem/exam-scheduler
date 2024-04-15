using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Models.Enums;
using ExamScheduler.Models;
using ExamScheduler.Tests.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ExamScheduler.Entities;

namespace ExamScheduler.Tests.Helpers
{
    internal class ParsingServiceTestsConfig
    {
        public static IFormFile ConvertToFormFile<T>(List<T> input)
        {
            var str = string.Join(Environment.NewLine, input.Select(i => i?.ToString()));
            var bytes = Encoding.UTF8.GetBytes(str);
            var stream = new MemoryStream(bytes);
            var file = new FormFile(stream, 0, stream.Length, "file", "file.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };
            return file;
        }

        public static List<MentorAvailability> GetMentorAvailabilities(ApplicationContext context, int courseId)
        {
            var mentors = context
                .Mentors
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .ToArray()
                ;

            var maxSlots = mentors.Length * Enum.GetValues(typeof(TimeSlot)).Length * 5;

            var course = context
                .Courses
                .FirstOrDefault(c => c.Id == courseId)
                ;

            if (course == null)
            {
                throw new TestConfigException($"Provided course id (value = {courseId}) does not exist.");
            }

            var students = context
                .Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .ToArray()
                ;

            if (students.Length == 0)
            {
                throw new TestConfigException("No students found in context.");
            }
            if (students.Length > maxSlots - 1)
            {
                throw new TestConfigException("Not enough mentors in context to provide realistic test input.");
            }

            var output = new List<MentorAvailability>();
            var examWeekMonday = DateTime
                .UtcNow
                .AddDays((7 + DayOfWeek.Monday - DateTime.Now.DayOfWeek) % 7)
                ;

            var random = new Random();
            for (int i = 0; i < maxSlots; i++)
            {
                var randMentor = mentors[random.Next(mentors.Length)];
                var date = DateOnly.FromDateTime(examWeekMonday.AddDays(i % 5));
                var timeSlot = (TimeSlot)((i / 5) % 3);

                while (output.Any(ma => ma.Mentor == randMentor && ma.Date == date && ma.TimeSlot == timeSlot))
                {
                    randMentor = mentors[random.Next(mentors.Length)];
                }

                output.Add(new MentorAvailability
                {
                    Mentor = randMentor,
                    Date = date,
                    TimeSlot = timeSlot
                });
            }
            return output;
        }

        public static List<MentorAvailability> InsertIncorrectMentorNames(List<MentorAvailability> avails)
        {
            avails.ForEach(d =>
            {
                d.Mentor.Name = d.Mentor.Name.Replace('a', 'u');
            });
            return avails;
        }

        public static List<StudentExamDetail> GetStudentExamDetails(ApplicationContext context, int courseId)
        {
            var course = context
                .Courses
                .FirstOrDefault(c => c.Id == courseId)
                ;

            if (course == null)
            {
                throw new TestConfigException($"Provided course id (value = {courseId}) does not exist.");
            }

            var students = context
                .Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .ToArray()
                ;

            if (students.Length == 0)
            {
                throw new TestConfigException("No students found in context.");
            }

            var random = new Random();

            var algoLangs = context
                .Mentors
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .SelectMany(m => m.AlgoLanguages)
                .Distinct()
                .ToList()
                ;

            if (algoLangs.Count == 0)
            {
                throw new TestConfigException("No algo languages available for exam scheduling.");
            }

            var algoLangCount = algoLangs.Count;

            algoLangs = algoLangs
                .Take(algoLangCount > 2 ? 3 : algoLangCount)
                .ToList()
                ;

            var output = new List<StudentExamDetail>();

            for (int i = 0; i < students.Length; i++)
            {
                output.Add(new StudentExamDetail
                {
                    Student = students[i],
                    AlgoLanguage = algoLangs[random.Next(algoLangs.Count)]
                });
            }

            return output;
        }

        public static List<StudentExamDetail> InsertIncorrectStudentNames(List<StudentExamDetail> details)
        {
            details.ForEach(d =>
            {
                d.Student.Name = d.Student.Name.Replace('a', 'u');
            });
            return details;
        }
    }
}
