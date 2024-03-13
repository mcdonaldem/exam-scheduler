using ExamScheduler.Contexts;

namespace ExamScheduler.Services
{
    public class ExamService(ApplicationContext context)
    {
        private ApplicationContext _context = context;
    }
}
