using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TrainSchedulesData
{
    public class DailyTrainSchedule
    {
        public IList<SingleTrainSchedule> ScheduledTrains { get; set; }

        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }
    }
}
