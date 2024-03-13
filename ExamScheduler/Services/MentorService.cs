using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using Microsoft.EntityFrameworkCore;

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
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .ToList()
                ;
        }

        public List<AlgoLanguage> GetActiveAlgoLanguages()
        {
            return _context.Mentors
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .SelectMany(m => m.AlgoLanguages)
                .Distinct()
                .ToList()
                ;
        }
    }
}
