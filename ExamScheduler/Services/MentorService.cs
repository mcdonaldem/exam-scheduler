using ExamScheduler.Contexts;
using ExamScheduler.Models;

namespace ExamScheduler.Services
{
    public class MentorService
    {
        private ApplicationContext _context;

        public MentorService(ApplicationContext context)
        {
            _context = context;
        }

        public List<Mentor> GetAllActive()
        {
            return _context.Mentors
                .Where(m => m.IsActive)
                .ToList()
                ;
        }
    }
}
