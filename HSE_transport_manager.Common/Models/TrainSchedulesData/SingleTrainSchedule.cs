using System;
using System.Collections.Generic;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class SingleTrainSchedule
    {
        public string TrainUid { get; set; }

        public IList<TrainStop> Stops { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}
