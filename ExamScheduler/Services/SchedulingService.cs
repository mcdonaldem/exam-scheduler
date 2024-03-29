using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using ExamScheduler.Exceptions;
using ExamScheduler.Models;
using ExamScheduler.Models.Enums;
using ExamScheduler.Services.Interfaces;

namespace ExamScheduler.Services
{
    public class SchedulingService(ApplicationContext context, IParsingService parsingService, IConfiguration configuration) : ISchedulingService
    {
        private ApplicationContext context = context;
        private IParsingService parsingService = parsingService;
        private IConfiguration configuration = configuration;

        public List<Exam> CreateSchedule(IFormFile mentorInfo, IFormFile studentInfo, int courseId)
        {
            var mentorAvails = parsingService.GetMentorAvailabilities(mentorInfo);
            var studentDetails = parsingService.GetStudentExamDetails(studentInfo, courseId);

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
                        AlgoLanguage = studentDetails[i].AlgoLanguage,
                        Student = studentDetails[i].Student,
                        Mentor = mentorSlot.Mentor,
                        Start = start,
                        End = start.Add(TimeSpan.Parse(configuration["ExamDuration"] ?? "2:00"))
                    });
                    mentorAvails.Remove(mentorSlot);
                }
                catch (SchedulingException se)
                {
                    throw new SchedulingException(se.Message);
                }
                catch (Exception e) when (e is not StackOverflowException && e is not OutOfMemoryException)
                {
                    throw new SchedulingException($"An error occured while scheduling exam for {studentDetails[i]?.Student?.Name}", e);
                }
            }
            context.Exams.AddRange(exams);
            context.SaveChanges();

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
            TimeSlot.Morning => TimeOnly.Parse(configuration["MorningStart"] ?? "09:30"),
            TimeSlot.EarlyAfternoon => TimeOnly.Parse(configuration["EarlyAfternoonStart"] ?? "13:00"),
            TimeSlot.LateAfternoon => TimeOnly.Parse(configuration["LateAfternoonStart"] ?? "15:30"),
            _ => throw new SchedulingException("No such time slot found.")
        };
    }
}
