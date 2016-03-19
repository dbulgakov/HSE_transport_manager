using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    public class DubkiBusData
    {
        public int Trip { get; set; }
        public DateTime DepartureTime { get; set; }
        public List<string> DayOfWeek { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }
}
