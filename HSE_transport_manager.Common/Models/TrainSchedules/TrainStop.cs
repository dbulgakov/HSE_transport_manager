using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TrainSchedules
{
    public class TrainStop
    {
        public TrainStation Station { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime ElapsedTime { get; set; }
    }
}
