using ExamScheduler.Contexts;
using ExamScheduler.Exceptions;
using ExamScheduler.Models;
using ExamScheduler.Models.Enums;

namespace ExamScheduler.Services
{
    public class SchedulingService
    {
        private ApplicationContext _context;
        private ParsingService _parsingService;

        public SchedulingService(ApplicationContext context, ParsingService parsingService)
        {
            _context = context;
            _parsingService = parsingService;
        }

        public IFormFile CreateSchedule(IFormFile mentorInfo, IFormFile studentInfo, int courseId)
        {
            var mentorAvails = _parsingService.GetMentorAvailabilities(mentorInfo);
            var studentDetails = _parsingService.GetStudentExamDetails(studentInfo, courseId);

            CheckInputForSchedulingFeasability(mentorAvails, studentDetails);

            studentDetails = SortStudentDetailsByPriority(studentDetails);
            mentorAvails = SortMentorAvailability(mentorAvails);
        }

        private void CheckInputForSchedulingFeasability(List<MentorAvailability> mentorAvails, 
            List<StudentExamDetail> studentDetails)
        {
            // Determine if there are enough slots in general
            if (mentorAvails.Count < studentDetails.Count)
            {
                throw new SchedulingException("Not enough exam slots to cover all students.");
            }

            //Determine if there are theoretically enough slots to cover each language
            var maxSlotsPerLang = studentDetails
                .Select(s => s.AlgoLanguage)
                .Distinct()
                .ToDictionary(r => r, r => mentorAvails.Count(m => m.Mentor.AlgoLanguages.Contains(r)));

            var studentLangCounts = studentDetails
                .GroupBy(s => s.AlgoLanguage)
                .ToDictionary(s => s.Key, s => s.Count())
                ;

            var uncovered = new List<AlgoLanguage>();
            foreach (var kvp in maxSlotsPerLang)
            {
                if (kvp.Value < studentLangCounts[kvp.Key])
                {
                    uncovered.Add(kvp.Key);
                }
            }
            if (uncovered.Count > 0)
            {
                throw new SchedulingException($"Not enough mentor availability to cover {String.Join(", ", uncovered.Select(u => u.Name))} exams.");
            }
        }

        private List<StudentExamDetail> SortStudentDetailsByPriority(List<StudentExamDetail> studentDetails)
        {
            // Priority is given to a language with a lesser number of students
            var studentLangCounts = studentDetails
                .GroupBy(s => s.AlgoLanguage)
                .ToDictionary(s => s.Key, s => s.Count())
                ;

            return studentDetails
                .OrderBy(s => studentLangCounts[s.AlgoLanguage])
                .ThenBy(s => s.Student.Name)
                .ToList()
                ;
        }

        private List<MentorAvailability> SortMentorAvailability(List<MentorAvailability> mentorAvails)
        {
            return mentorAvails
                .OrderBy(m => m.Date)
                .ThenBy(m => m.TimeSlot)
                .ToList()
                ;
        }

        private TimeOnly GetStartTime(TimeSlot timeSlot) => timeSlot switch
        {
            TimeSlot.Morning => TimeOnly.Parse(Environment.GetEnvironmentVariable("MORNING_START")),
            TimeSlot.EarlyAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("EARLY_AFTERNOON_START")),
            TimeSlot.LateAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("LATE_AFTERNOON_START")),
            _ => throw new SchedulingException("No such time slot found.")
        };
    }
}
