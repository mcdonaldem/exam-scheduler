﻿using ExamScheduler.Entities;
using ExamScheduler.Entities.Enums;
using ExamScheduler.Exceptions;
using ExamScheduler.Extensions;
using ExamScheduler.Models;

namespace ExamScheduler.Services
{
    public class ParsingService(MentorService mentorService, StudentService studentService)
    {
        private MentorService _mentorService = mentorService;
        private StudentService _studentService = studentService;
        private char[] validDelimiters = [',', ';', '|'];

        public List<MentorAvailability> GetMentorAvailabilities(IFormFile file)
        {
            var mentors = _mentorService.GetAllActive();

            var content = file.ReadAsList();

            var output = new List<MentorAvailability>();
            for (int i = 0; i < content.Count; i++)
            {
                var data = ValidateData(content[i], i, 3);
                var mentor = (Mentor)ValidatePerson(mentors, data[0]);

                try
                {
                    output.Add(new MentorAvailability
                    {
                        Mentor = mentor,
                        Date = DateOnly.Parse(data[1]),
                        TimeSlot = (TimeSlot)Enum.Parse(typeof(TimeSlot), data[2])
                    });
                }
                catch (Exception)
                {
                    throw new InvalidFileDataException("Invalid date or time format.");
                }
            }
            return output;
        }

        public List<StudentExamDetail> GetStudentExamDetails(IFormFile file, int courseId)
        {
            var students = _studentService.GetAllByCourse(courseId);

            var availableAlgoLangs = _mentorService.GetActiveAlgoLanguages();

            var content = file.ReadAsList();

            var output = new List<StudentExamDetail>();
            for (int i = 0; i < content.Count; i++)
            {
                var data = ValidateData(content[i], i, 2);
                var student = (Student)ValidatePerson(students, data[0]);

                try
                {
                    output.Add(new StudentExamDetail
                    {
                        Student = student,
                        AlgoLanguage = availableAlgoLangs.First(a => a.Name == data[1])
                    });
                }
                catch (Exception)
                {
                    throw new InvalidFileDataException("Invalid algo language.");
                }
            }
            return output;
        }

        private string[] ValidateData(string line, int index, int expectedColumns)
        {
            var data = line.Split(validDelimiters);
            if (data.Length != expectedColumns)
            {
                throw new InvalidFileDataException($"Line {index + 1} contains {data.Length} columns instead of {expectedColumns}.");
            }
            return data;
        }

        private Person ValidatePerson(IEnumerable<Person> people, string name)
        {
            var person = people?.FirstOrDefault(m => m.Name == name);
            if (person is null)
            {
                throw new InvalidFileDataException($"{people?.GetType()?.GetGenericArguments()?.FirstOrDefault()?.Name} with name \"{name}\" not found.");
            }
            return person;
        }
    }
}
