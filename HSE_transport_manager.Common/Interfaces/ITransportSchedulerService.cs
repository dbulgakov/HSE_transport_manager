﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models.TrainSchedules;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITransportSchedulerService
    {
         Task<DailyTrainSchedule> GetScheduleAsync(string startingStation, string endingStation);
    }
}
