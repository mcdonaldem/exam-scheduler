using Microsoft.AspNetCore.Diagnostics;

namespace ExamScheduler.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToErrorInfoString(this Exception exception)
        {
            var output = GetAllExceptionInfo(exception);
            output += $"Stack trace: {exception!.StackTrace}";
            return output;
        }

        private static string GetAllExceptionInfo(Exception exception)
        {
            var output = $"Exception info:" +
                $"{Environment.NewLine}" +
                $"Type: {exception!.GetType().Name}" +
                $"{Environment.NewLine}" +
                $"Message: {exception!.Message}" +
                $"{Environment.NewLine}"
                ;

            if (exception!.InnerException is not null)
            {
                output += GetAllExceptionInfo(exception.InnerException);
            }

            return output;
        }
    }
}
