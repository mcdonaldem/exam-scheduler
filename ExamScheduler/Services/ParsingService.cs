using System.Globalization;
using ExamScheduler.Contexts;
using ExamScheduler.Exceptions;
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
            // Get all currently active mentors from DB
            var mentors = mentorService.GetAllActive();

            // Get content from file
            var content = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    content.Add(reader.ReadLine());
                }
            }

            // Parse each line
            var output = new List<MentorAvailability>();
            foreach (var line in content)
            {
                var data = line.Split(validDelimiters);
                if (data.Length != 3)
                {
                    throw new InvalidMentorDataException("Uploaded file contains incorrect number of columns.");
                }

                var mentor = mentors.FirstOrDefault(m => m.Name == data[0]);
                if (mentor is null)
                {
                    throw new InvalidMentorDataException("Mentor not found");
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
