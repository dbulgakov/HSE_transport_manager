﻿using HSE_transport_manager.Common.Models.TrainSchedulesData;
using MSDatabaseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService
{
    public interface IDatabaseService
    {
        void RefreshTrainSchedule(DailyTrainSchedule trainSchedule);
        void RemoveTrainSchedule();
        List<QueryResult> GetRoute(string fromPoint, string toPoint, DateTime queryDate);
    }
}
