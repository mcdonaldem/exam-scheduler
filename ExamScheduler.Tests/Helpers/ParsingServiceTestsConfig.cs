using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ExamScheduler.Tests.Helpers
{
    internal class ParsingServiceTestsConfig
    {
        public static IFormFile ConvertToFormFile<T>(List<T> input)
        {
            var bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, input.Select(i => i?.ToString())));
            var stream = new MemoryStream(bytes);
            var file = new FormFile(stream, 0, stream.Length, "file", "file.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };
            return file;
        }
    }
}
