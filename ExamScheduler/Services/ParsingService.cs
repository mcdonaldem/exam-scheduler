using System.Text;
using ExamScheduler.Entities;
using ExamScheduler.Exceptions;
using ExamScheduler.Extensions;
using ExamScheduler.Models;
using ExamScheduler.Models.Enums;
using ExamScheduler.Services.Interfaces;

namespace ExamScheduler.Services
{
    public class ParsingService(IMentorService mentorService, IStudentService studentService) : IParsingService
    {
        private IMentorService mentorService = mentorService;
        private IStudentService studentService = studentService;
        private char[] validDelimiters = [',', ';', '|'];

        public List<MentorAvailability> GetMentorAvailabilities(IFormFile file)
        {
            string[] content = ValidateFile(file);

            var mentors = mentorService.GetAllActive();

            var output = new List<MentorAvailability>();
            for (int i = 0; i < content.Length; i++)
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
                    throw new InvalidFileDataException("Invalid date or time given.");
                }
            }
            return output;
        }

        public List<StudentExamDetail> GetStudentExamDetails(IFormFile file, int courseId)
        {
            var content = ValidateFile(file);

            var students = studentService.GetAllByCourse(courseId);

            var availableAlgoLangs = mentorService.GetActiveAlgoLanguages();

            var output = new List<StudentExamDetail>();
            for (int i = 0; i < content.Length; i++)
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
                catch (Exception e) when (e is not StackOverflowException && e is not OutOfMemoryException)
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

        private string[] ValidateFile(IFormFile file)
        {
            if(file == null)
            {
                throw new SchedulingException("No file provided.");
            }
            else if(file.ContentType != "text/csv")
            {
                throw new SchedulingException("Incorrect file type provided.");
            }
            else if(file.Length == 0)
            {
                throw new SchedulingException("Provided file is empty.");
            }
            else
            {
                return file.ReadAsArray();
            }
        }
    }
}
