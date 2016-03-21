using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TrainSchedulesData;
using System;
using System.Collections.Generic;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface IDatabaseService
    {
        void RefreshTrainSchedule(DailyTrainSchedule trainSchedule);
        void RemoveTrainSchedule();
        List<Coordinate> GetCoordinates(string fromPoint, string toPoint);
        List<QueryResult> GetRoute(string fromPoint, string toPoint, DateTime queryDate);
    }
}
