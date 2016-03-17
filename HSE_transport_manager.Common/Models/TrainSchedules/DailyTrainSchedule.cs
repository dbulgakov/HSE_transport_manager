﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TrainSchedules
{
    public class DailyTrainSchedule
    {
        public List<SingleTrainSchedule> ScheduledTrains { get; set; }

        public TrainStation DepartureStation { get; set; }

        public TrainStation ArrivalStation { get; set; }
    }
}
