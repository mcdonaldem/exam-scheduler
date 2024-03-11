using ExamScheduler.Contexts;

namespace ExamScheduler.Services
{
    public class ExamService
    {
        private ApplicationContext _context;

        public ExamService(ApplicationContext context)
        {
            _context = context;
        }
    }
}
