using System;
using System.Collections.Generic;
using HSE_transport_manager.Common.Interfaces;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class SingleTrainSchedule
    {
        public string TrainUid { get; set; }

        public IList<TrainStop> Stops { get; set; }

        public DateTime DepartureTime { get; set; }

        public Transport TransportType { get; set; }
    }
}
