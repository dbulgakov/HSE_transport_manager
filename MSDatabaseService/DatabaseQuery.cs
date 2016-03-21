using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TrainSchedulesData;
using MSDatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MSDatabaseService
{
    public class DatabaseQuery : IDatabaseService
    {
        private Context context = new Context();

        private static CultureInfo culture = new CultureInfo("ru-RU");
        private DateTimeFormatInfo dtfi = culture.DateTimeFormat;
        private string dayAbbreviation;

        private TransportRoute transportRoute;
        private List<QueryResult> routesResult;
        private List<Route> routeList;
        private List<TransportRoute> transportList;
        private bool check;

        private int minutes;
        private int hours;


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
                            Station = context.LocalTrainStations.FirstOrDefault(s => s.Code.Equals(stop.StationCode)),
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
                        DepartureStation = context.LocalTrainStations.FirstOrDefault(s => s.Code.Equals(trainSchedule.DepartureStation)),
                        ArrivalStation = context.LocalTrainStations.FirstOrDefault(s => s.Code.Equals(trainSchedule.ArrivalStation)),
                        Stops = stopsList,
                        Type = context.TransportTypes.FirstOrDefault(t => t.Name.Equals(train.TransportType.ToString()))
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
            dayAbbreviation = dtfi.GetShortestDayName(queryDate.DayOfWeek).ToUpper();
            routesResult = new List<QueryResult>();
            routeList = new List<Route>();
            transportList = new List<TransportRoute>();

            if (context.Dormitories.Where(d => d.Name.Equals(fromPoint)).Select(d => d.Name) != null && context.HSEBuildings.Where(b => b.Name.Equals(toPoint)).Select(b => b.Name) != null)
            {
                var subwayStationHSE = context.SubwayStations.Where(s=> s.HSEBuilding.Any(b=> b.Name==toPoint)==true).Select(s=> s.Name).ToList();
                
                
                //Dubki - Works
                if (context.Dormitories.Where(r => r.Name == fromPoint).Select(s => s.CheckDubkiBus).Single())
                {
                    check=true;
                    transportList.Add(new TransportRoute
                        {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = queryDate.AddMinutes(25),
                            FromPoint = fromPoint,
                            ToPoint = "Остановка автобуса",
                            TransportType = "OnFoot"
                        });

                    minutes = 20;
                    if (queryDate.Minute + 20 > 60)
                    {
                        hours++;
                        minutes = queryDate.Minute - 40;
                    }

                    var dubkiQuery = context.DubkiBusesSchedule.Where(b => b.From == "Дубки" && (b.DepartureTime.Hour > queryDate.Hour+hours || b.DepartureTime.Hour == queryDate.Hour+hours && b.DepartureTime.Minute >= queryDate.Minute + minutes) && b.DayOfWeek.Any(d => d.Name == dayAbbreviation) == true)
                                                          .Select(b => new
                                                            {
                                                              DepartureTime= b.DepartureTime,
                                                              To=b.To,
                                                              TransportType=b.Type.Name
                                                            })
                                                          .First();

                    transportList.Add(new TransportRoute
                        {
                            DepartureTime = dubkiQuery.DepartureTime,
                            ElapsedTime = dubkiQuery.DepartureTime.AddMinutes(35),
                            FromPoint = "Дубки - Автобусная остановка",
                            ToPoint = dubkiQuery.To,
                            TransportType = dubkiQuery.TransportType
                        });

                }

                //PublicTransport 
                if (context.PublicTransportSchedule.Where(t => t.Dormitory.Any(d => d.Name == fromPoint) == true).Select(t => t.DepartureTime).Count() != 0)
                {
                    minutes = 20;
                    if (queryDate.Minute + 20 > 60)
                    {
                        hours++;
                        minutes = queryDate.Minute - 40;
                    }
                    var publicTransportQuery = context.PublicTransportSchedule.Where(p => p.Dormitory.Any(d => d.Name == fromPoint) == true && p.DayOfWeek.Any(d => d.Name == dayAbbreviation) == true && (p.DepartureTime.Hour > queryDate.Hour + hours || p.DepartureTime.Hour == queryDate.Hour + hours && p.DepartureTime.Minute >= queryDate.Minute + minutes))
                                             .Select(t => new
                                                          {
                                                              From = t.From,
                                                              To = t.To,
                                                              Number = t.Number,
                                                              DepartureTime = t.DepartureTime,
                                                              TransportType = context.TransportTypes.Where(y => y.Id == t.Type.Id).Select(y => y.Name).FirstOrDefault(),
                                                              Price = t.Price.Price
                                                          })
                                             .First();

                    //Tram - Works
                    if (publicTransportQuery.TransportType == "Tram" && toPoint == "Кирпичная 33")
                    {
                        transportList.Add(new TransportRoute
                        {
                            DepartureTime = queryDate.AddMinutes(20),
                            ElapsedTime = queryDate.AddMinutes(30),
                            FromPoint = fromPoint,
                            ToPoint = publicTransportQuery.From,
                            TransportType = "OnFoot"
                        });

                        transportList.Add(new TransportRoute
                        {
                            DepartureTime = publicTransportQuery.DepartureTime,
                            ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(15),
                            FromPoint = publicTransportQuery.From,
                            ToPoint = publicTransportQuery.To,
                            Price = publicTransportQuery.Price,
                            TransportType = publicTransportQuery.TransportType,
                            Number = publicTransportQuery.Number
                        });

                        transportList.Add(new TransportRoute
                            {
                                DepartureTime = publicTransportQuery.DepartureTime.AddMinutes(15),
                                ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(30),
                                FromPoint = publicTransportQuery.To,
                                ToPoint = toPoint,
                                TransportType = "OnFoot"
                            });

                        routeList.Add(new Route
                        {
                            Transport = transportList
                        });

                    }
                    //Bus - Works
                    if (publicTransportQuery.TransportType == "Bus")
                    {

                        foreach (var subwayStation in subwayStationHSE)
                        {
                            transportList = new List<TransportRoute>();

                            transportList.Add(new TransportRoute
                            {
                                DepartureTime = queryDate.AddMinutes(20),
                                ElapsedTime = queryDate.AddMinutes(30),
                                FromPoint = fromPoint,
                                ToPoint = publicTransportQuery.From,
                                TransportType = "OnFoot"
                            });

                            transportList.Add(new TransportRoute
                            {
                                DepartureTime = publicTransportQuery.DepartureTime,
                                ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(40),
                                FromPoint = publicTransportQuery.From,
                                ToPoint = publicTransportQuery.To,
                                Price = publicTransportQuery.Price,
                                TransportType = publicTransportQuery.TransportType,
                                Number = publicTransportQuery.Number,
                            });


                            transportRoute = GetRouteSubStToSubSt(publicTransportQuery.To, subwayStation, publicTransportQuery.DepartureTime.AddMinutes(40));
                            transportList.Add(transportRoute);

                            transportList.Add(new TransportRoute
                            {
                                DepartureTime = transportRoute.ElapsedTime,
                                ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
                                FromPoint = transportRoute.ToPoint,
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
                //Subway - Works
                var subwayStationQuery = context.SubwayStations.Where(s => s.Dormitory.Any(d => d.Name == fromPoint) == true)
                                                         .Select(l => l.Name).ToList();

                    if (subwayStationQuery.Count != 0)
                    {
                        foreach (var stationHSE in subwayStationHSE)
                        {
                            transportList = new List<TransportRoute>();
                            foreach (var station in subwayStationQuery)
                            {
                                transportList = new List<TransportRoute>();
                                transportList.Add(new TransportRoute
                                {
                                    DepartureTime = queryDate.AddMinutes(20),
                                    ElapsedTime = queryDate.AddMinutes(30),
                                    FromPoint = fromPoint,
                                    ToPoint = station,
                                    TransportType = "OnFoot"
                                });

                                transportRoute = GetRouteSubStToSubSt(station, stationHSE, queryDate.AddMinutes(30));
                                transportList.Add(transportRoute);

                                transportList.Add(new TransportRoute
                                {
                                    DepartureTime = transportRoute.ElapsedTime,
                                    ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
                                    FromPoint = transportRoute.ToPoint,
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

                    // LocalTrain -  FILL AND CHECK!!!!!!
                    var localStation = context.Dormitories.Where(r => r.Name == fromPoint)
                                                              .Select(r => r.LocalTrainStation.Name).FirstOrDefault();

                    //if (localStation != null)
                    //{
                    //    minutes = 20;
                    //    if (check)
                    //    {
                    //        fromPoint = "Автовокзал";
                    //        minutes = 55;
                    //    }

                    //    if (queryDate.Minute + minutes > 60)
                    //    {
                    //        hours++;
                    //        minutes = queryDate.Minute - 40;
                    //    }

                    //    var localStationsQuery = context.LocalTrainsSchedule.Where(s => s.DepartureStation.Name == localStation && (s.DepartureTime.Hour > queryDate.Hour + hours || s.DepartureTime.Hour == queryDate.Hour + hours && s.DepartureTime.Minute >= queryDate.Minute + minutes)).Select(r => new
                    //         {
                    //             DepartureTime = r.DepartureTime,
                    //             Stops = r.Stops,
                    //             Type = r.Type.Name
                    //         }).First();

                    //    foreach (var stationHSE in subwayStationHSE)
                    //    {
                    //        transportList = new List<TransportRoute>();
                    //        foreach (var stationStop in localStationsQuery.Stops)
                    //        {
                    //            transportList = new List<TransportRoute>();
                    //            transportList.Add(new TransportRoute
                    //            {
                    //                DepartureTime = queryDate.AddMinutes(minutes),
                    //                ElapsedTime = queryDate.AddMinutes(minutes + 10),
                    //                FromPoint = fromPoint,
                    //                ToPoint = localStation,
                    //                TransportType = "OnFoot"
                    //            });

                    //            transportList.Add(new TransportRoute
                    //            {
                    //                DepartureTime = queryDate.AddMinutes(minutes + 10),
                    //                ElapsedTime = stationStop.ElapsedTime,
                    //                FromPoint = localStation,
                    //                ToPoint = stationStop.Station.Name,
                    //                TransportType = localStationsQuery.Type,
                    //                Price = context.LocalTrainPrices.Where(p => p.StationFrom.Name == localStation && p.StationTo.Name == stationStop.Station.Name).Select(p => p.Price).Single()
                    //            });

                    //            if (stationStop.Station.Name == "Белорусский вокзал") stationStop.Station.Name = "Белорусская";
                    //            if (stationStop.Station.Name == "Кунцево") stationStop.Station.Name = "Кунцевская";

                    //            transportRoute = GetRouteSubStToSubSt(stationStop.Station.Name, stationHSE, queryDate.AddMinutes(minutes + 10));

                    //            transportList.Add(new TransportRoute
                    //            {
                    //                DepartureTime = queryDate.AddMinutes(minutes + 10),
                    //                ElapsedTime = transportRoute.ElapsedTime,
                    //                FromPoint = transportRoute.FromPoint,
                    //                ToPoint = transportRoute.ToPoint,
                    //                TransportType = transportRoute.TransportType
                    //            });

                    //            transportList.Add(new TransportRoute
                    //            {
                    //                DepartureTime = transportRoute.ElapsedTime,
                    //                ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
                    //                FromPoint = transportRoute.ToPoint,
                    //                ToPoint = toPoint,
                    //                TransportType = "OnFoot"
                    //            });

                    //            routeList.Add(new Route
                    //            {
                    //                Transport = transportList
                    //            });
                    //        }
                    //  }
                   // }
                
                }

            routesResult.Add( new QueryResult
            {
                DeparturePoint=fromPoint,
                ArrivalPoint=toPoint,
                Routes=routeList
            });

            return routesResult;
        }

        public TransportRoute GetRouteSubStToSubSt(string stationFrom, string stationTo, DateTime queryDate)
        {

            return new TransportRoute
                         {
                             ElapsedTime = queryDate.AddMinutes(30),
                             FromPoint = stationFrom,
                             ToPoint = stationTo,
                             Price = 50,
                             TransportType = "Subway"
                         };
        }

    }
}
