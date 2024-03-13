using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ExamScheduler.Entities;

namespace ExamScheduler.Services
{
    public class OutputSerializerService
    {
        public byte[] ToByteArray(List<Exam> exams)
        {
            return Encoding.UTF32.GetBytes(String.Join(Environment.NewLine, exams.Select(e => e.ToFileString())));
        }

        public Stream ToStream(List<Exam> exams)
        {
            return new MemoryStream(ToByteArray(exams));
        }
    }
}
