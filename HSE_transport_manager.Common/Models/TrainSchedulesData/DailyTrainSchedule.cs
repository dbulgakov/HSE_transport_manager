using System.Collections.Generic;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class DailyTrainSchedule
    {
        public IList<SingleTrainSchedule> ScheduledTrains { get; set; }

        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }
    }
}
