using ExamScheduler.Entities;

namespace ExamScheduler.Services.Interfaces
{
    public interface IStudentService
    {
        List<Student> GetAllByCourse(int courseId);
    }
}