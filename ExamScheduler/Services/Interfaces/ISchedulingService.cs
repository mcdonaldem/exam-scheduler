using ExamScheduler.Entities;

namespace ExamScheduler.Services.Interfaces
{
    public interface ISchedulingService
    {
        List<Exam> CreateSchedule(IFormFile mentorInfo, IFormFile studentInfo, int courseId);
    }
}