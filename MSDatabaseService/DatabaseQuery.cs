using HSE_transport_manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService
{
    class DatabaseQuery
    {
        private Context context = new Context();

        //public void RefreshTrainSchedule(DailyTrainSchedule trainSchedule)
        //{
        //    if (context.LocalTrainsSchedule!=null)
        //        RemoveTrainSchedule();

        //    var stopsList = new List<LocalTrainStop();
        //    foreach (var train in trainSchedule.SingleTrainSchedule)
        //    {
        //        foreach (var stop in train.Stops)
        //            stopsList.Add(
        //                new LocalTrainStop
        //                {
        //                    Station = context.LocalTrainStations.Single(s => s.Code == stop.Code),
        //                    ArrivalTime = stop.ArrivalTime,
        //                    ElapsedTime = stop.ElapsedTime
        //                });

        //        context.LocalTrainStops.Add(stopsList);
        //        context.SaveChanges();

        //        context.LocalTrainsSchedule.Add(
        //            new LocalTrainSchedule
        //            {
        //                DepartureTime=train.DepartureTime,
        //                Uid=train.Uid,
        //                DepartureStation=context.LocalTrainStations.Single( s => s.Code==train.DepartureStation),
        //                ArrivalStation=context.LocalTrainStations.Single( s => s.Code==train.ArrivalStation),
        //                Stops=stopsList
        //            });
        //        context.SaveChanges();
        //    } 
        //}

        public void RemoveTrainSchedule()
        {
            foreach (var train in context.LocalTrainsSchedule)
                context.LocalTrainsSchedule.Remove(train);
            foreach (var stop in context.LocalTrainStops)
                context.LocalTrainStops.Remove(stop);
        }


    }
}
