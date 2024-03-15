using ExamScheduler.Entities;
using ExamScheduler.Models;

namespace ExamScheduler.Services.Interfaces
{
    public interface IParsingService
    {
        List<MentorAvailability> GetMentorAvailabilities(IFormFile file);
        List<StudentExamDetail> GetStudentExamDetails(IFormFile file, int courseId);
    }
}