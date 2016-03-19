using HSE_transport_manager.Common.Models.TrainSchedulesData;
using HSE_transport_manager.Entities;
using MSDatabaseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService 
{
    public class DatabaseQuery : IDatabaseService
    {
        private Context context = new Context();

        public void RefreshTrainSchedule(DailyTrainSchedule trainSchedule)
        {
            //if (context.LocalTrainsSchedule!=null)
            //    RemoveTrainSchedule();

            //var stopsList = new List<LocalTrainStop>();
            //foreach (var train in trainSchedule.ScheduledTrains)
            //{
            //    foreach (var stop in train.Stops)
            //        stopsList.Add(
            //            new LocalTrainStop
            //            {
            //                Station = context.LocalTrainStations.Single(s => s.Code.Equals(stop.StationCode)),
            //                ArrivalTime = stop.ArrivalTime,
            //                ElapsedTime = stop.ElapsedTime
            //            });
            //    foreach(var item in stopsList)
            //        context.LocalTrainStops.Add(item);
            //    context.SaveChanges();

            //    context.LocalTrainsSchedule.Add(
            //        new LocalTrainSchedule
            //        {
            //            DepartureTime=train.DepartureTime,
            //            Uid=train.TrainUid,
            //            DepartureStation=context.LocalTrainStations.Single( s => s.Code.Equals(trainSchedule.DepartureStation)),
            //            ArrivalStation=context.LocalTrainStations.Single( s => s.Code.Equals(trainSchedule.ArrivalStation)),
            //            Stops=stopsList,
            //            Type=context.TransportTypes.Single(t=> t.Name.Equals(train.TransportType.Transport.GetName()))
            //        });
            //    context.SaveChanges();
            //} 
        }

        public void RemoveTrainSchedule()
        {
            foreach (var train in context.LocalTrainsSchedule)
                context.LocalTrainsSchedule.Remove(train);
            foreach (var stop in context.LocalTrainStops)
                context.LocalTrainStops.Remove(stop);
        }

        public List<QueryResult> GetRoute()
        {

            return null;
        }

    }
}
