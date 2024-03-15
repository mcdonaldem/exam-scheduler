using ExamScheduler.Entities;

namespace ExamScheduler.Services.Interfaces
{
    public interface IMentorService
    {
        List<AlgoLanguage> GetActiveAlgoLanguages();
        List<Mentor> GetAllActive();
    }
}