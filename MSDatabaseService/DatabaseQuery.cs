using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Entities;

namespace MSDatabaseService
{
    class DatabaseQuery
    {
        static private Context context = new Context();


        //static void RefreshLocalTrainsSchedule(SOMETHING data)
        //{
        //    if (context.LocalTrainsSchedule!=null || context.LocalTrainStops!=null)
        //        RemoveLocalTrainsSchedule();

        //    foreach (var smth in data)
        //    {
        //        List<LocalTrainStop> listStops = new List<LocalTrainStop>();
        //        foreach ( var smth2 in smth)
        //            listStops.Add( 
        //                new LocalTrainStop 
        //                {
        //                    Station=context.LocalTrainStations.Single(s=>s.Name==smth2.Name),
        //                    ArrivalTime=smth2.ArrivalTime,
        //                    ElapsedTime=????????????
        //                });
        //        context.LocalTrainStops.Add(listStops);
        //        context.SaveChanges();


        //        context.LocalTrainsSchedule.Add(
        //            new LocalTrainSchedule
        //            {
        //              DepartureTime=smth.DepartureTime,
        //              Uid=smth.Uid,
        //              DepartureStation=context.LocalTrainStations.Single(s=> s.Name=="Одинцово"),
        //              Stops=listStops
        //            });
        //        context.SaveChanges();
        //    }
        //}

        //static void RemoveLocalTrainsSchedule()
        //{
        //    foreach (var train in context.LocalTrainsSchedule)
        //        context.LocalTrainsSchedule.Remove(train);
        //    context.SaveChanges();

        //    foreach (var stop in context.LocalTrainStops)
        //        context.LocalTrainStops.Remove(stop);
        //    context.SaveChanges();
        //}

        //static SMTH GetRoute(string startingPoint, string finishPoint)
        //{
        //    if (context.Dormitories.Any(q => q.Name == startingPoint))
        //    {
        //        return null;
        //    }
        //    else if (context.HSEBuildings.Any(q => q.Name == startingPoint))
        //    {
        //        return null;
        //    }
        //    else return null;
        //}
    }
}
