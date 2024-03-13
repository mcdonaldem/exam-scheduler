using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduler.Services
{
    public class StudentService(ApplicationContext context)
    {
        private ApplicationContext _context = context;

        public List<Student> GetAllByCourse(int courseId)
        {
            if(_context.Courses.Any(c => c.Id == courseId))
            {
                return _context.Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.Student)
                    .ToList()
                    ;
            }
            throw new ArgumentException("Invalid course id.");
        }
    }
}
