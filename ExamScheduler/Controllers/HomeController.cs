using ExamScheduler.Entities;
using ExamScheduler.Exceptions;
using ExamScheduler.Models.Enums;
using ExamScheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduler.Controllers
{
    public class HomeController(OutputSerializerService outputSerializerService, SchedulingService schedulingService, IConfiguration configuration) : Controller
    {
        private OutputSerializerService outputSerializerService = outputSerializerService;
        private SchedulingService schedulingService = schedulingService;
        private IConfiguration configuration = configuration;

        [HttpGet("/schedule")]
        public IActionResult GetFile(IFormFile mentorInfo, IFormFile studentInfo, int courseId)
        {
            var content = schedulingService.CreateSchedule(mentorInfo, studentInfo, courseId);

            try
            {
                Func<List<Exam>, dynamic> output = (OutputSerialzation)int.Parse(configuration["OutputSerialization"]) switch
                {
                    OutputSerialzation.ByteArray => outputSerializerService.ToByteArray,
                    OutputSerialzation.Stream => outputSerializerService.ToStream,
                    _ => throw new ConfigurationException("Output serialization type not recognized.")
                };
                return File(output(content), "text/csv", "scheduled_exams.csv");
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Output serialization type not configured.", e);
            }
        }
    }
}
