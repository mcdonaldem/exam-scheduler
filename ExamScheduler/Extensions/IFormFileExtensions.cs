namespace ExamScheduler.Extensions
{
    public static class IFormFileExtensions
    {
        public static List<string> ReadAsList(this IFormFile file)
        {
            var content = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() > -1)
                {
                    content.Add(reader.ReadLine());
                }
            }
            return content;
        }
    }
}
