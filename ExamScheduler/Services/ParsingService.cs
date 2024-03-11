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
        private MentorService mentorService;
        private char[] validDelimiters;

        public ParsingService(ApplicationContext context, MentorService mentorService)
        {
            _context = context;
            this.mentorService = mentorService;
            validDelimiters = [',', ';', '|'];
        }

        public List<MentorAvailability> GetMentorAvailabilities(IFormFile file)
        {
            var mentors = mentorService.GetAllActive();

            var content = file.ReadAsList();

            var output = new List<MentorAvailability>();
            for(int i = 0; i < content.Count; i++)
            {
                var data = content[i].Split(validDelimiters);
                if (data.Length != 3)
                {
                    throw new InvalidMentorDataException($"Line {i + 1} contains incorrect number of columns.");
                }

                var mentor = mentors.FirstOrDefault(m => m.Name == data[0]);
                if (mentor is null)
                {
                    throw new InvalidMentorDataException($"Mentor with name \"{data[0]}\" not found");
                }

                try
                {
                    output.Add(new MentorAvailability
                    {
                        Mentor = mentor,
                        Date = DateOnly.Parse(data[1]),
                        TimeSlot = (TimeSlot)Enum.Parse(typeof(TimeSlot), data[2])
                    });
                }
                catch (Exception e)
                {
                    throw new InvalidMentorDataException(e.Message);
                }
            }
            return output;
        }
    }
}
