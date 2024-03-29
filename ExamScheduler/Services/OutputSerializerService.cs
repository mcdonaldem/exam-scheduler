using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ExamScheduler.Entities;
using ExamScheduler.Services.Interfaces;

namespace ExamScheduler.Services
{
    public class OutputSerializerService : IOutputSerializerService
    {
        public byte[] ToByteArray(List<Exam> exams)
        {
            return Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, exams.Select(e => e.ToFileString())));
        }

        public Stream ToStream(List<Exam> exams)
        {
            return new MemoryStream(ToByteArray(exams));
        }
    }
}
