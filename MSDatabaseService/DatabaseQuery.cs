using HSE_transport_manager.Common.Models.TrainSchedulesData;
using HSE_transport_manager.Entities;
using MSDatabaseService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService 
{
    public class DatabaseQuery : IDatabaseService
    {
        private Context context = new Context();
        private CultureInfo culture = new System.Globalization.CultureInfo("ru-RU");

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

        public List<QueryResult> GetRoute(string fromPoint, string toPoint, DateTime queryDate)
        {

            var routesResult = new List<QueryResult>();

            if (context.Dormitories.Single(d => d.Name.Equals(fromPoint)) != null && context.HSEBuildings.Single(b => b.Name.Equals(toPoint)) != null)
            {

                if (context.Dormitories.Join(context.PublicTransportSchedule, d => d.Id, t => t.Trip, (d, s) => new { d, s })
                                       .Where(r => r.d.Name == fromPoint)
                                       .Select(r => new { Number = r.s.Number }) != null && toPoint == "Кирпичная 33")
                {
                    var query = context.Dormitories.Join(context.PublicTransportSchedule, d => d.Id, t => t.Number, (d, s) => new { d, s })
                                               .Where(r => r.d.Name == fromPoint && r.s.DepartureTime.Subtract(queryDate).Minutes >= 20)
                                               .Join(context.DayofWeek, h => h.s.Trip, f => f.Id, (h, f) => new { h, f })
                                               .Where(r => r.h.s.DayOfWeek.Any(d => d.Name == queryDate.DayOfWeek.ToString("ddd", culture).ToUpper()))
                                               .Join(context.TransportTypes, p => p.h.s.Trip, y => y.Id, (p, y) => new { p, y })
                                               .Select(r => new Transport
                                                                {
                                                                    FromPoint = r.p.h.s.From,
                                                                    ToPoint = r.p.h.s.To,
                                                                    DepartureTime = r.p.h.s.DepartureTime,
                                                                    ElapsedTime = r.p.h.s.DepartureTime.AddMinutes(15),
                                                                    TransportType = r.y.Name,
                                                                    Price = r.p.h.s.Price.Price
                                                                }).Single();

                    if (query.TransportType == "Tram")
                    {
                        routesResult.Add(
                            new QueryResult
                            {
                                DeparturePoint = fromPoint,
                                ArrivalPoint = toPoint,
                                Transport = new List<Transport>
                                                {
                                                    new Transport
                                                        {
                                                            DepartureTime=query.DepartureTime,
                                                            ElapsedTime=query.ElapsedTime,
                                                            FromPoint=query.FromPoint,
                                                            ToPoint=query.ToPoint,
                                                            Price=query.Price,
                                                            TransportType=query.TransportType
                                                        }
                                                }
                            });
                        return routesResult;
                    }
                    else
                    {
                        return routesResult;
                    }
                }
                else
                {
                    return routesResult;
                }
            }
            return routesResult;
        }
    }
}
