namespace ExamScheduler.Extensions
{
    public static class IFormFileExtensions
    {
        public static string[] ReadAsArray(this IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var bytes = new byte[file.Length];
                fileStream.Read(bytes, 0, (int)file.Length);
                return Convert.ToBase64String(bytes)
                    .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    ;
            }
        }
    }
}
