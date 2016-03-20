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
        private static CultureInfo culture = new CultureInfo("ru-RU");
        private DateTimeFormatInfo dtfi = culture.DateTimeFormat;
        private Transport tr;


        public void UploadElapsedTimeSubway()
        {

        }

        public void RefreshTrainSchedule(DailyTrainSchedule trainSchedule)
        {
            if (context.LocalTrainsSchedule != null)
                RemoveTrainSchedule();

            var stopsList = new List<LocalTrainStop>();
            foreach (var train in trainSchedule.ScheduledTrains)
            {
                foreach (var stop in train.Stops)
                    stopsList.Add(
                        new LocalTrainStop
                        {
                            Station = context.LocalTrainStations.Single(s => s.Code.Equals(stop.StationCode)),
                            ArrivalTime = stop.ArrivalTime,
                            ElapsedTime = stop.ElapsedTime
                        });
                foreach (var item in stopsList)
                    context.LocalTrainStops.Add(item);
                context.SaveChanges();

                context.LocalTrainsSchedule.Add(
                    new LocalTrainSchedule
                    {
                        DepartureTime = train.DepartureTime,
                        Uid = train.TrainUid,
                        DepartureStation = context.LocalTrainStations.Single(s => s.Code.Equals(trainSchedule.DepartureStation)),
                        ArrivalStation = context.LocalTrainStations.Single(s => s.Code.Equals(trainSchedule.ArrivalStation)),
                        Stops = stopsList,
                        Type = context.TransportTypes.Single(t => t.Name.Equals(train.TransportType.ToString()))
                    });
                context.SaveChanges();
            } 
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
            var routeList = new List<Route>();
            List<Transport> transportList = new List<Transport>();
            bool check=false;
            int time = 0;

            if (context.Dormitories.Single(d => d.Name.Equals(fromPoint)) != null && context.HSEBuildings.Single(b => b.Name.Equals(toPoint)) != null)
            {
                var stationHSE = context.HSEBuildings.Join(context.SubwayStations, d => d.Id, s => s.Id, (d, s) => new { d, s })
                                       .Where(r => r.d.Name == toPoint)
                                       .Select(r => r.s.Name).Single();

                if (context.Dormitories.Where(r => r.Name == toPoint).Select(s=> s.CheckDubkiBus).Single()==true)
                {
                    check=true;
                    transportList.Add(new Transport
                        {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = queryDate.AddMinutes(25),
                            FromPoint = fromPoint,
                            ToPoint = "Остановка автобуса",
                            TransportType = "OnFoot"
                        });


                    var dubki=context.DubkiBusesSchedule.Where(r => r.From == "Дубки" && r.DepartureTime.Subtract(queryDate).Minutes >= 25)
                                               .Join(context.DayofWeek, h => h.Trip, f => f.Id, (h, f) => new { h, f })
                                               .Select(q => new
                                               {
                                                   DepartureTime=q.h.DepartureTime,
                                                   To=q.h.From,
                                                   Type=q.h.Type.Name
                                               })
                                               .Single();

                    transportList.Add(new Transport
                        {
                            DepartureTime = queryDate.AddMinutes(30),
                            ElapsedTime = queryDate.AddMinutes(60),
                            FromPoint = fromPoint,
                            ToPoint = "Одинцово",
                            TransportType = dubki.Type
                        });

                    time=35;
                }
                //PublicTransport OK
                if (context.Dormitories.Join(context.PublicTransportSchedule, d => d.Id, t => t.Trip, (d, s) => new { d, s })
                                       .Where(r => r.d.Name == fromPoint)
                                       .Select(r => new { Number = r.s.Number }) != null)
                {
                    var query = context.Dormitories.Join(context.PublicTransportSchedule, d => d.Id, t => t.Trip, (d, s) => new { d, s })
                                               .Where(r => r.d.Name == fromPoint && r.s.DepartureTime.Subtract(queryDate).Minutes >= 20)
                                               .Join(context.DayofWeek, h => h.s.Trip, f => f.Id, (h, f) => new { h, f })
                                               .Where(r => r.h.s.DayOfWeek.Any(d => d.Name == dtfi.GetShortestDayName(queryDate.DayOfWeek).ToUpper()))
                                               .Select(r => new Transport
                                                                {
                                                                    FromPoint = r.h.s.From,
                                                                    ToPoint = r.h.s.To,
                                                                    Number=r.h.s.Number,
                                                                    DepartureTime = r.h.s.DepartureTime,
                                                                    TransportType = context.TransportTypes.Where(t=> t.Id==r.h.s.Type.Id).Select(s=> s.Name).Single(),
                                                                    Price = r.h.s.Price.Price
                                                                }).Single();
                    //Tram
                    if (query.TransportType == "Tram" && toPoint == "Кирпичная 33")
                    {
                        transportList.Add(new Transport
                        {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = queryDate.AddMinutes(30),
                            FromPoint = fromPoint,
                            ToPoint = query.FromPoint,
                            TransportType = "OnFoot"
                        });

                        transportList.Add(new Transport
                        {
                            DepartureTime = query.DepartureTime,
                            ElapsedTime = query.DepartureTime.AddMinutes(15),
                            FromPoint = query.FromPoint,
                            ToPoint = query.ToPoint,
                            Price = query.Price,
                            TransportType = query.TransportType,
                            Number=query.Number
                        });

                        transportList.Add(new Transport
                            {
                                DepartureTime = query.DepartureTime.AddMinutes(15),
                                ElapsedTime = query.DepartureTime.AddMinutes(30),
                                FromPoint = query.ToPoint,
                                ToPoint = toPoint,
                                TransportType = "OnFoot"
                            });

                        routeList.Add(new Route 
                        {
                           Transport=transportList
                        });
                        
                    }
                    //Bus
                    else if (query.TransportType == "Bus")
                    {
                        transportList.Add(new Transport
                        {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = query.ElapsedTime.AddMinutes(30),
                            FromPoint = fromPoint,
                            ToPoint = query.FromPoint,
                            TransportType = "OnFoot"
                        });

                        transportList.Add(new Transport
                                          {
                                            DepartureTime=query.DepartureTime,
                                            ElapsedTime=query.ElapsedTime,
                                            FromPoint=query.FromPoint,
                                            ToPoint=query.ToPoint,
                                            Price=query.Price,
                                            TransportType=query.TransportType,
                                            Number=query.Number,
                                          });

                        

                        tr = GetRouteSubStToSubSt(query.ToPoint,query.TransportType, stationHSE, query.ElapsedTime);
                        transportList.Add(tr);

                        transportList.Add(new Transport
                        {
                            DepartureTime = tr.ElapsedTime,
                            ElapsedTime = tr.ElapsedTime.AddMinutes(10),
                            FromPoint = tr.ToPoint,
                            ToPoint = toPoint,
                            TransportType = "OnFoot"
                        });

                        routeList.Add(new Route
                        {
                            Transport = transportList
                        });
                    }
                    //Subway
                    else
                    {
                        var stationDorm = context.Dormitories.Join(context.SubwayStations, d => d.Id, s => s.Id, (d, s) => new { d, s })
                                                             .Where(r => r.d.Name == fromPoint)
                                                             .Join(context.TransportTypes, p => p.s.Id, y => y.Id, (p, y) => new { p, y })
                                                             .Select(r => new
                                                                {
                                                                    Station = r.p.s.Name,
                                                                    TransportType = r.y.Name,
                                                                });

                        if (stationDorm != null)
                        {
                            
                            foreach (var st in stationDorm)
                            {
                                transportList.Add(new Transport
                                {
                                    DepartureTime = queryDate.AddMinutes(20),
                                    ElapsedTime = queryDate.AddMinutes(30),
                                    FromPoint = fromPoint,
                                    ToPoint = st.Station,
                                    TransportType = "OnFoot"
                                });

                                tr = GetRouteSubStToSubSt(st.Station, st.TransportType, stationHSE, queryDate);
                                transportList.Add(tr);

                                transportList.Add(new Transport
                                {
                                    DepartureTime = tr.ElapsedTime,
                                    ElapsedTime = tr.ElapsedTime.AddMinutes(10),
                                    FromPoint = tr.ToPoint,
                                    ToPoint = toPoint,
                                    TransportType = "OnFoot"
                                });

                                routeList.Add(new Route
                                {
                                    Transport = transportList
                                });
                            }
                        }

                        var localStationDorm = context.Dormitories.Where(r => r.Name == toPoint)
                                                                  .Select(r => r.LocalTrainStation.Name).Single();

                        if (localStationDorm!= null)
                        {
                           if (check) fromPoint="Автовокзал";
                           var  queryTR = context.LocalTrainsSchedule.Where(s => s.DepartureStation == context.LocalTrainStations.Single(t => t.Name == "Одинцово")).Select(r => new
                                {
                                    DepartureTime=r.DepartureTime,
                                    Stops=r.Stops,
                                    Type = context.TransportTypes.Where(t => t.Id == r.Type.Id).Select(s => s.Name).Single()
                                }).Single();

                            foreach(var item in queryTR.Stops)
                            {
                                transportList.Add(new Transport
                                {
                                    DepartureTime = queryDate.AddMinutes(20+time),
                                    ElapsedTime = queryDate.AddMinutes(30+time),
                                    FromPoint = fromPoint,
                                    ToPoint = item.Station.Name,
                                    TransportType = "OnFoot"
                                });

                                if (item.Station.Name == "Белорусский вокзал") item.Station.Name = "Белорусская";
                                if (item.Station.Name == "Кунцево") item.Station.Name = "Кунцевская";

                                tr=GetRouteSubStToSubSt(item.Station.Name, queryTR.Type, stationHSE, queryDate.AddMinutes(30+time));

                                transportList.Add(new Transport
                                {
                                    DepartureTime = tr.ElapsedTime,
                                    ElapsedTime = tr.ElapsedTime.AddMinutes(10),
                                    FromPoint = tr.ToPoint,
                                    ToPoint = toPoint,
                                    TransportType = "OnFoot"
                                });

                                routeList.Add(new Route
                                {
                                    Transport = transportList
                                });
                            } 
                        }
                    }
                }
            }
            
            routesResult.Add( new QueryResult
            {
                DeparturePoint=fromPoint,
                ArrivalPoint=toPoint,
                Routes=routeList
            });

            return routesResult;
        }

        Transport GetRouteSubStToSubSt(string stationFrom, string transType, string stationTo, DateTime queryDate)
        {
            return new Transport
                         {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = queryDate.AddMinutes(20+context.SubwayRouteElapsedTime.Where(t => t.StationFrom == context.SubwayStations.Single(s => s.Name == stationFrom) && t.StationTo == context.SubwayStations.Single(s => s.Name == stationTo))
                                                                              .Select(t => t.ElapsedTime).Single()),
                            FromPoint = stationFrom,
                            ToPoint = stationTo,
                            Price = 50,
                            TransportType = transType
                         };             
        }

    }
}
