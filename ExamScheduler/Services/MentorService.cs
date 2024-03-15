using ExamScheduler.Contexts;
using ExamScheduler.Entities;
using ExamScheduler.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduler.Services
{
    public class MentorService(ApplicationContext context) : IMentorService
    {
        private ApplicationContext context = context;

        public List<Mentor> GetAllActive()
        {
            return context.Mentors
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .ToList()
                ;
        }

        public List<AlgoLanguage> GetActiveAlgoLanguages()
        {
            return context.Mentors
                .Include(m => m.AlgoLanguages)
                .Where(m => m.IsActive)
                .SelectMany(m => m.AlgoLanguages)
                .Distinct()
                .ToList()
                ;
        }
    }
}
