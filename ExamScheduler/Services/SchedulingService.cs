using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using ExamScheduler.Entities.Enums;
using ExamScheduler.Exceptions;
using ExamScheduler.Models;

namespace ExamScheduler.Services
{
    public class SchedulingService(ApplicationContext context, ParsingService parsingService)
    {
        private ApplicationContext _context = context;
        private ParsingService _parsingService = parsingService;

        public List<Exam> CreateSchedule(IFormFile mentorInfo, IFormFile studentInfo, int courseId)
        {
            var mentorAvails = _parsingService.GetMentorAvailabilities(mentorInfo);
            var studentDetails = _parsingService.GetStudentExamDetails(studentInfo, courseId);

            CheckInputForSchedulingFeasability(mentorAvails, studentDetails);

            studentDetails = SortStudentDetailsByPriority(studentDetails);
            mentorAvails = SortMentorAvailability(mentorAvails);

            var exams = new List<Exam>();
            for (int i = 0; i < studentDetails.Count; i++)
            {
                try
                {
                    var mentorSlot = mentorAvails.First(m => m.Mentor.AlgoLanguages.Contains(studentDetails[i].AlgoLanguage));
                    var start = new DateTime(mentorSlot.Date, GetStartTime(mentorSlot.TimeSlot));
                    exams.Add(new Exam
                    {
                        Student = studentDetails[i].Student,
                        Mentor = mentorSlot.Mentor,
                        Start = start,
                        End = start.Add(TimeSpan.Parse(Environment.GetEnvironmentVariable("EXAM_DURATION") ?? "2:00"))
                    });
                    mentorAvails.Remove(mentorSlot);
                }
                catch(Exception e)
                {
                    throw new SchedulingException($"An error occured while scheduling exam for {studentDetails[i]?.Student?.Name}", e);
                }
            }
            _context.Exams.AddRange(exams);
            _context.SaveChanges();

            return exams;
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
            TimeSlot.Morning => TimeOnly.Parse(Environment.GetEnvironmentVariable("MORNING_START") ?? "09:30"),
            TimeSlot.EarlyAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("EARLY_AFTERNOON_START") ?? "13:00"),
            TimeSlot.LateAfternoon => TimeOnly.Parse(Environment.GetEnvironmentVariable("LATE_AFTERNOON_START") ?? "15:30"),
            _ => throw new SchedulingException("No such time slot found.")
        };
    }
}
