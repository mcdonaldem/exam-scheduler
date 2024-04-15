namespace ExamScheduler.Extensions
{
    public static class IFormFileExtensions
    {
        public static string[] ReadAsArray(this IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return reader.ReadToEnd()
                    .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    ;
            }
        }
    }
}
