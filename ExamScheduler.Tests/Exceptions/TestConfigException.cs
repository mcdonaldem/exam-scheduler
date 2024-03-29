using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExamScheduler.Tests.Exceptions
{
    public class TestConfigException : Exception
    {
        public TestConfigException()
        {
        }

        public TestConfigException(string? message) : base(message)
        {
        }

        public TestConfigException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
