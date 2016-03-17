using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class TrainStop
    {
        public string StationCode { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime ElapsedTime { get; set; }
    }
}
