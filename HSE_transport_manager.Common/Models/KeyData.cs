using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models
{
    public class SettingsData
    {
        public string MonitoringServiceKey { get; set; }
        public string ScheduleServiceKey { get; set; }
        public string TaxiServiceKey { get; set; }
        public string BotServiceKey { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
