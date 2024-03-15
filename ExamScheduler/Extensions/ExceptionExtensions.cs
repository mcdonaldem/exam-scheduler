using Microsoft.AspNetCore.Diagnostics;

namespace ExamScheduler.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToErrorInfoString(this Exception exception)
        {
            var output = $"Exception info:" +
                $"{Environment.NewLine}" +
                $"Type: {exception!.GetType().Name}" +
                $"{Environment.NewLine}" +
                $"Message: {exception!.Message}" +
                $"{Environment.NewLine}"
                ;

            if(exception!.InnerException is not null)
            {
                output += ToErrorInfoString(exception.InnerException);
            }

            output += $"Stack trace: {exception!.StackTrace}";
            return output;
        }
    }
}
