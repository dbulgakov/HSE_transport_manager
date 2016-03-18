using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class SingleTrainSchedule
    {
        public string TrainUid { get; set; }

        public IList<TrainStop> Stops { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}
