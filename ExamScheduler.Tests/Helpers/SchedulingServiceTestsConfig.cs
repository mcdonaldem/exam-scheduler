using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamScheduler.Tests.Helpers
{
    public class SchedulingServiceTestsConfig
    {
        public static Dictionary<string, string?> GetConfigurationKVPs()
        {
            return new Dictionary<string, string?>
            {
                {"ExamDuration", "2:30"},
                {"MorningStart", "09:00"},
                {"EarlyAfternoonStart","13:30"},
                {"LateAfternoonStart","15:30"}
            };
        }
    }
}
