using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Contexts;
using ExamScheduler.Services.Interfaces;
using ExamScheduler.Services;
using Moq;

namespace ExamSchedulerTests.Services
{
    public class ParsingServiceTests
    {
        private ApplicationContext context;
        private Mock<IMentorService> mentorService;
        private Mock<IStudentService> studentService;
        private ParsingService service;

        public ParsingServiceTests()
        {
        }
    }
}
