using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models
{
    public class TrainSchedule
    {
        public string DepartureStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime ArrivalTime { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
    }
}
