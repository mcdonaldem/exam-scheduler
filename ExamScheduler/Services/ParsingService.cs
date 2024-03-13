using ExamScheduler.Contexts;
using ExamScheduler.Exceptions;
using ExamScheduler.Extensions;
using ExamScheduler.Models;
using ExamScheduler.Models.Enums;

namespace ExamScheduler.Services
{
    public class ParsingService
    {
        private ApplicationContext _context;
        private MentorService _mentorService;
        private StudentService _studentService;
        private char[] validDelimiters;

        public ParsingService(ApplicationContext context, MentorService mentorService, StudentService studentService)
        {
            _context = context;
            _mentorService = mentorService;
            validDelimiters = [',', ';', '|'];
            _studentService = studentService;
        }

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
                        AlgoLanguage = (AlgoLanguage)Enum.Parse(typeof(AlgoLanguage), data[1])
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
