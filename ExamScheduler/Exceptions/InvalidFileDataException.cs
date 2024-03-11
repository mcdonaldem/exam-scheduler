namespace ExamScheduler.Exceptions
{
    public class InvalidFileDataException : Exception
    {
        public InvalidFileDataException(string? message) : base(message)
        {
        }
    }
}
