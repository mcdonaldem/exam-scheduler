namespace ExamScheduler.Exceptions
{
    public class SchedulingException : Exception
    {
        public SchedulingException(string? message) : base(message)
        {
        }

        public SchedulingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
