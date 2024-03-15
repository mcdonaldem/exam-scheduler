using ExamScheduler.Entities;

namespace ExamScheduler.Services.Interfaces
{
    public interface IOutputSerializerService
    {
        byte[] ToByteArray(List<Exam> exams);
        Stream ToStream(List<Exam> exams);
    }
}