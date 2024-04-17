using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamScheduler.Entities;
using ExamScheduler.Models.Enums;

namespace ExamScheduler.Tests.Helpers
{
    internal class MentorAvailabilityForTesting
    {
        public string Mentor { get; set; }
        public string Date { get; set; }
        public string TimeSlot { get; set; }

        public override string ToString()
        {
            return $"{Mentor};{Date};{TimeSlot}";
        }
    }
}
