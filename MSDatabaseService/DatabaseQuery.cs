using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TrainSchedulesData;
using MSDatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MSDatabaseService
{
    public class DatabaseQuery : IDatabaseService
    {
        private Context context = new Context();

        private static CultureInfo culture = new CultureInfo("ru-RU");
        private DateTimeFormatInfo dtfi = culture.DateTimeFormat;
        private string dayAbbreviation;

        private TransportRoute transportRoute;
        private List<Route> routeList;
        private List<TransportRoute> transportList;
        private List<TransportRoute> transportList2;

        private string stationFix;

        private int minutes;
        private int hours;


        public void RefreshTrainSchedule(DailyTrainSchedule trainSchedule)
        {
            if (context.LocalTrainsSchedule.Count() != 0)
                RemoveTrainSchedule();
            List<LocalTrainStop> stopsList;
            List<DateTime> stopsListArrival;
            foreach (var train in trainSchedule.ScheduledTrains)
            {
                stopsList = new List<LocalTrainStop>();
                stopsListArrival = new List<DateTime>();
                foreach (var stop in train.Stops)
                    if (stop.StationCode.Equals("s2000006") || stop.StationCode.Equals("s9600721") || stop.StationCode.Equals("s9600821") || stop.StationCode.Equals("s9601666") || stop.StationCode.Equals("s9601728"))
                    {
                        stopsList.Add(
                                new LocalTrainStop
                                {
                                    Station = context.LocalTrainStations.Single(s => s.Code.Equals(stop.StationCode)),
                                    ArrivalTime = stop.ArrivalTime,
                                    ElapsedTime = stop.ArrivalTime.AddMinutes(stop.ElapsedTime.Minute)
                                });
                        stopsListArrival.Add(stopsList[stopsList.Count - 1].ArrivalTime);
                    }
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
                        Stops = context.LocalTrainStops.Where(s => stopsListArrival.Contains(s.ArrivalTime)).ToList(),
                        Type = context.TransportTypes.Single(t => t.Name.Equals(train.TransportType.ToString()))
                    });
                context.SaveChanges();
            }
        }

        public void RemoveTrainSchedule()
        {
            foreach (var stop in context.LocalTrainStops)
                context.LocalTrainStops.Remove(stop);
            context.SaveChanges();
            foreach (var train in context.LocalTrainsSchedule)
                context.LocalTrainsSchedule.Remove(train);
            context.SaveChanges();
        }



        public Coordinate GetCoordinates(string point)
        {
            //if (context.Dormitories.Any(d => d.Name.Equals(point)))
            //    return new Coordinate
            //    {
            //        Latitude = double.Parse(context.Dormitories.Where(d => d.Name.Equals(point)).Select(d => d.Latitude).Single().ToString(), culture),
            //        Longitude = double.Parse(context.Dormitories.Where(d => d.Name.Equals(point)).Select(d => d.Longitude).Single().ToString(), culture),
            //    };

            //else if (context.HSEBuildings.Any(b => b.Name.Equals(point)))
            //    return new Coordinate
            //    {
            //        Latitude = double.Parse(context.HSEBuildings.Where(d => d.Name.Equals(point)).Select(d => d.Latitude).Single().ToString(), culture),
            //        Longitude = double.Parse(context.HSEBuildings.Where(d => d.Name.Equals(point)).Select(d => d.Longitude).Single().ToString(), culture),
            //    };

            //else if (context.SubwayStations.Any(s => s.Name.Equals(point)))
            //    return new Coordinate
            //    {
            //        Latitude = double.Parse(context.SubwayStations.Where(d => d.Name.Equals(point)).Select(d => d.Latitude).Single().ToString(), culture),
            //        Longitude = double.Parse(context.SubwayStations.Where(d => d.Name.Equals(point)).Select(d => d.Longitude).Single().ToString(), culture),
            //    };

            //else throw new ArgumentException();
            return null;
        }



        public QueryResult GetRoute(string fromPoint, string toPoint, DateTime queryDate)
        {
        //    if (context.Dormitories.Any(d => d.Name.Equals(fromPoint)) && context.HSEBuildings.Any(h => h.Name.Equals(toPoint)))
        //    {
        //        dayAbbreviation = dtfi.GetShortestDayName(queryDate.DayOfWeek).ToUpper();
        //        routeList = new List<Route>();
        //        transportList = new List<TransportRoute>();
        //        transportList2 = new List<TransportRoute>();
        //        string dubkiTo = " ";
        //        DateTime time = queryDate;

        //        var subwayStationHSE = context.SubwayStations.Where(s => s.HSEBuilding.Any(b => b.Name.Equals(toPoint))).Select(s => s.Name).ToList();

        //        //Dubki - Works
        //        if (context.Dormitories.Where(r => r.Name.Equals(fromPoint)).Select(s => s.CheckDubkiBus).Single())
        //        {
        //            dubkiTo = "Одинцово";

        //            minutes = 20;
        //            if (queryDate.Minute + 20 > 60)
        //            {
        //                hours++;
        //                minutes = queryDate.Minute - 40;
        //            }

        //            var dubkiQuery = context.DubkiBusesSchedule.Where(b => b.From.Equals("Дубки") && (b.DepartureTime.Hour > queryDate.Hour + hours || b.DepartureTime.Hour == queryDate.Hour + hours && b.DepartureTime.Minute >= queryDate.Minute + minutes) && b.DayOfWeek.Any(d => d.Name.Equals(dayAbbreviation)))
        //                                                  .Select(b => new
        //                                                  {
        //                                                      DepartureTime = b.DepartureTime,
        //                                                      To = b.To,
        //                                                      TransportType = b.Type.Name
        //                                                  })
        //                                                  .First();



        //            if (!dubkiQuery.To.Equals("Одинцово") && (queryDate.Hour < 1 || queryDate.Hour == 5 && queryDate.Minute >= 30 || queryDate.Hour > 5))
        //            {
        //                dubkiTo = dubkiQuery.To;
        //                foreach (var subwayStation in subwayStationHSE)
        //                {

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = queryDate.AddMinutes(20),
        //                        ElapsedTime = queryDate.AddMinutes(25),
        //                        FromPoint = fromPoint,
        //                        ToPoint = "Остановка автобуса",
        //                        TransportType = "OnFoot"
        //                    });

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = dubkiQuery.DepartureTime,
        //                        ElapsedTime = dubkiQuery.DepartureTime.AddMinutes(35),
        //                        FromPoint = "Дубки - Автобусная остановка",
        //                        ToPoint = dubkiQuery.To,
        //                        TransportType = dubkiQuery.TransportType
        //                    });


        //                    transportRoute = GetRouteSubStToSubSt(dubkiQuery.To, subwayStation, dubkiQuery.DepartureTime.AddMinutes(35));
        //                    transportList.Add(transportRoute);

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = transportRoute.ElapsedTime,
        //                        ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
        //                        FromPoint = transportRoute.ToPoint,
        //                        ToPoint = toPoint,
        //                        TransportType = "OnFoot"
        //                    });

        //                    routeList.Add(new Route
        //                    {
        //                        Transport = transportList
        //                    });
        //                }

        //            }
        //            else
        //            {

        //                transportList2.Add(new TransportRoute
        //                {
        //                    DepartureTime = queryDate.AddMinutes(20),
        //                    ElapsedTime = queryDate.AddMinutes(25),
        //                    FromPoint = fromPoint,
        //                    ToPoint = "Остановка автобуса",
        //                    TransportType = "OnFoot"
        //                });

        //                transportList2.Add(new TransportRoute
        //                {
        //                    DepartureTime = dubkiQuery.DepartureTime,
        //                    ElapsedTime = dubkiQuery.DepartureTime.AddMinutes(35),
        //                    FromPoint = "Дубки - Автобусная остановка",
        //                    ToPoint = dubkiQuery.To,
        //                    TransportType = dubkiQuery.TransportType
        //                });
        //                time = dubkiQuery.DepartureTime.AddMinutes(35);
        //            }
        //        }

        //        minutes = 20;

        //        if (queryDate.Minute + 20 > 60)
        //        {
        //            hours++;
        //            minutes = queryDate.Minute - 40;
        //        }

        //        //PublicTransport 
        //        if (context.PublicTransportSchedule.Any(t => t.Dormitory.Any(d => d.Name.Equals(fromPoint)) && context.PublicTransportSchedule.Any(p => p.Dormitory.Any(d => d.Name.Equals(fromPoint)) && p.DayOfWeek.Any(d => d.Name.Equals(dayAbbreviation)) && (p.DepartureTime.Hour > queryDate.Hour + hours || p.DepartureTime.Hour == queryDate.Hour + hours && p.DepartureTime.Minute >= queryDate.Minute + minutes))))
        //        {
        //            var publicTransportQuery = context.PublicTransportSchedule.Where(p => p.Dormitory.Any(d => d.Name.Equals(fromPoint)) && p.DayOfWeek.Any(d => d.Name.Equals(dayAbbreviation)) && (p.DepartureTime.Hour > queryDate.Hour + hours || p.DepartureTime.Hour == queryDate.Hour + hours && p.DepartureTime.Minute >= queryDate.Minute + minutes))
        //                                     .Select(t => new
        //                                     {
        //                                         From = t.From,
        //                                         To = t.To,
        //                                         Number = t.Number,
        //                                         DepartureTime = t.DepartureTime,
        //                                         TransportType = context.TransportTypes.Where(y => y.Id.Equals(t.Type.Id)).Select(y => y.Name).FirstOrDefault(),
        //                                         Price = t.Price.Price
        //                                     })
        //                                     .First();

        //            //Tram - Works
        //            if (publicTransportQuery.TransportType == "Tram" && toPoint == "Кирпичная 33")
        //            {
        //                transportList.Add(new TransportRoute
        //                {
        //                    DepartureTime = queryDate.AddMinutes(20),
        //                    ElapsedTime = queryDate.AddMinutes(30),
        //                    FromPoint = fromPoint,
        //                    ToPoint = publicTransportQuery.From,
        //                    TransportType = "OnFoot"
        //                });

        //                transportList.Add(new TransportRoute
        //                {
        //                    DepartureTime = publicTransportQuery.DepartureTime,
        //                    ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(15),
        //                    FromPoint = publicTransportQuery.From,
        //                    ToPoint = publicTransportQuery.To,
        //                    Price = publicTransportQuery.Price,
        //                    TransportType = publicTransportQuery.TransportType,
        //                    Number = publicTransportQuery.Number
        //                });

        //                transportList.Add(new TransportRoute
        //                {
        //                    DepartureTime = publicTransportQuery.DepartureTime.AddMinutes(15),
        //                    ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(30),
        //                    FromPoint = publicTransportQuery.To,
        //                    ToPoint = toPoint,
        //                    TransportType = "OnFoot"
        //                });

        //                routeList.Add(new Route
        //                {
        //                    Transport = transportList
        //                });

        //            }
        //            //Bus - Works
        //            if (publicTransportQuery.TransportType == "Bus" && (queryDate.Hour < 1 || queryDate.Hour == 5 && queryDate.Minute >= 30 || queryDate.Hour > 5))
        //            {

        //                foreach (var subwayStation in subwayStationHSE)
        //                {
        //                    transportList = new List<TransportRoute>();

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = queryDate.AddMinutes(20),
        //                        ElapsedTime = queryDate.AddMinutes(30),
        //                        FromPoint = fromPoint,
        //                        ToPoint = publicTransportQuery.From,
        //                        TransportType = "OnFoot"
        //                    });

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = publicTransportQuery.DepartureTime,
        //                        ElapsedTime = publicTransportQuery.DepartureTime.AddMinutes(70),
        //                        FromPoint = publicTransportQuery.From,
        //                        ToPoint = publicTransportQuery.To,
        //                        Price = publicTransportQuery.Price,
        //                        TransportType = publicTransportQuery.TransportType,
        //                        Number = publicTransportQuery.Number,
        //                    });


        //                    transportRoute = GetRouteSubStToSubSt(publicTransportQuery.To, subwayStation, publicTransportQuery.DepartureTime.AddMinutes(70));
        //                    transportList.Add(transportRoute);

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = transportRoute.ElapsedTime,
        //                        ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
        //                        FromPoint = transportRoute.ToPoint,
        //                        ToPoint = toPoint,
        //                        TransportType = "OnFoot"
        //                    });

        //                    routeList.Add(new Route
        //                    {
        //                        Transport = transportList
        //                    });
        //                }
        //            }
        //        }
        //        //Subway - Works

        //        if (context.SubwayStations.Any(s => s.Dormitory.Any(d => d.Name.Equals(fromPoint)) && (queryDate.Hour < 1 || queryDate.Hour == 5 && queryDate.Minute >= 30 || queryDate.Hour > 5)))
        //        {
        //            var subwayStationQuery = context.SubwayStations.Where(s => s.Dormitory.Any(d => d.Name.Equals(fromPoint)))
        //                                                 .Select(l => l.Name).ToList();

        //            foreach (var stationHSE in subwayStationHSE)
        //            {
        //                transportList = new List<TransportRoute>();
        //                foreach (var station in subwayStationQuery)
        //                {
        //                    transportList = new List<TransportRoute>();
        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = queryDate.AddMinutes(20),
        //                        ElapsedTime = queryDate.AddMinutes(30),
        //                        FromPoint = fromPoint,
        //                        ToPoint = station,
        //                        TransportType = "OnFoot"
        //                    });

        //                    transportRoute = GetRouteSubStToSubSt(station, stationHSE, queryDate.AddMinutes(30));
        //                    transportList.Add(transportRoute);

        //                    transportList.Add(new TransportRoute
        //                    {
        //                        DepartureTime = transportRoute.ElapsedTime,
        //                        ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
        //                        FromPoint = transportRoute.ToPoint,
        //                        ToPoint = toPoint,
        //                        TransportType = "OnFoot"
        //                    });

        //                    routeList.Add(new Route
        //                    {
        //                        Transport = transportList
        //                    });
        //                }
        //            }
        //        }

        //        // LocalTrain - Works

        //        if ((context.Dormitories.Where(r => r.Name.Equals(fromPoint))
        //                                                  .Select(r => new
        //                                                  {
        //                                                      StationName = r.LocalTrainStation.Name,
        //                                                      Code = r.LocalTrainStation.Code
        //                                                  }).Single() != null || dubkiTo.Equals("Одинцово")) && (queryDate.Hour < 1 || queryDate.Hour == 5 && queryDate.Minute >= 30 || queryDate.Hour > 5))
        //        {
        //            var localStation = !dubkiTo.Equals("Одинцово") ? context.Dormitories.Where(r => r.Name.Equals(fromPoint))
        //                                                  .Select(r => new
        //                                                  {
        //                                                      StationName = r.LocalTrainStation.Name,
        //                                                      Code = r.LocalTrainStation.Code
        //                                                  }).Single()
        //                                                  :
        //                                                  context.LocalTrainStations.Where(r => r.Name.Equals(dubkiTo))
        //                                                  .Select(r => new
        //                                                  {
        //                                                      StationName = r.Name,
        //                                                      Code = r.Code
        //                                                  }).Single();


        //            minutes = 20;
        //            if (dubkiTo.Equals("Одинцово"))
        //            {
        //                fromPoint = "Автовокзал";
        //                time = time.AddMinutes(5);
        //            }
        //            else time = queryDate.AddMinutes(minutes);


        //            if (context.LocalTrainsSchedule.Any(s => s.DepartureStation.Name.Equals(localStation.StationName) && (s.DepartureTime.Hour > time.Hour || s.DepartureTime.Hour == time.Hour && s.DepartureTime.Minute >= time.Minute)))
        //            {
        //                var localStationsQuery = context.LocalTrainsSchedule.Where(s => s.DepartureStation.Name.Equals(localStation.StationName) && (s.DepartureTime.Hour > time.Hour || s.DepartureTime.Hour == time.Hour && s.DepartureTime.Minute >= time.Minute))
        //                .Select(r => new
        //                {
        //                    DepartureTime = r.DepartureTime,
        //                    Stops = r.Stops.Select(t => new
        //                    {
        //                        ArrivalTime = t.ArrivalTime,
        //                        ElapsedTime = t.ElapsedTime,
        //                        StationName = t.Station.Name,
        //                        Code = t.Station.Code
        //                    }).ToList(),
        //                    Type = r.Type.Name
        //                }).First();

        //                foreach (var stationHSE in subwayStationHSE)
        //                {
        //                    transportList = new List<TransportRoute>();
        //                    if (transportList2.Count != 0)
        //                    {
        //                        transportList.Add(transportList2[0]);
        //                        transportList.Add(transportList2[1]);
        //                    }
        //                    foreach (var stationStop in localStationsQuery.Stops)
        //                    {
        //                        transportList = new List<TransportRoute>();
        //                        if (transportList2.Count != 0)
        //                        {
        //                            transportList.Add(transportList2[0]);
        //                            transportList.Add(transportList2[1]);
        //                        }
        //                        if (transportList2.Count == 0)
        //                            transportList.Add(new TransportRoute
        //                            {
        //                                DepartureTime = queryDate.AddMinutes(minutes),
        //                                ElapsedTime = queryDate.AddMinutes(minutes + 10),
        //                                FromPoint = fromPoint,
        //                                ToPoint = localStation.StationName,
        //                                TransportType = "OnFoot"
        //                            });
        //                        else
        //                            transportList.Add(new TransportRoute
        //                            {
        //                                DepartureTime = time.AddMinutes(-5),
        //                                ElapsedTime = time,
        //                                FromPoint = fromPoint,
        //                                ToPoint = localStation.StationName,
        //                                TransportType = "OnFoot"
        //                            });

        //                        transportList.Add(new TransportRoute
        //                        {
        //                            DepartureTime = localStationsQuery.DepartureTime,
        //                            ElapsedTime = stationStop.ElapsedTime,
        //                            FromPoint = localStation.StationName,
        //                            ToPoint = stationStop.StationName,
        //                            TransportType = localStationsQuery.Type,
        //                            Price = localStationsQuery.Type.Equals("Suburban") ? context.LocalTrainPrices.Where(p => p.StationFrom.Code.Equals(localStation.Code) && p.StationTo.Code.Equals(stationStop.Code)).Select(p => p.Price).Single() : context.LocalTrainPrices.Where(p => p.StationFrom.Code.Equals(localStation.Code) && p.StationTo.Code.Equals(stationStop.Code)).Select(p => p.Price).Single() * 2
        //                        });

        //                        stationFix = stationStop.StationName;
        //                        if (stationStop.StationName.Equals("Белорусский вокзал")) stationFix = "Белорусская";
        //                        if (stationStop.StationName.Equals("Кунцево")) stationFix = "Кунцевская";

        //                        if (stationStop.ElapsedTime.Hour < 1 || stationStop.ElapsedTime.Hour == 5 && stationStop.ElapsedTime.Minute >= 30 || queryDate.Hour > 5)
        //                        {
        //                            transportRoute = GetRouteSubStToSubSt(stationFix, stationHSE, stationStop.ElapsedTime.AddMinutes(10));

        //                            transportList.Add(new TransportRoute
        //                            {
        //                                DepartureTime = stationStop.ElapsedTime.AddMinutes(10),
        //                                ElapsedTime = transportRoute.ElapsedTime,
        //                                FromPoint = transportRoute.FromPoint,
        //                                ToPoint = transportRoute.ToPoint,
        //                                TransportType = transportRoute.TransportType,
        //                                Price = transportRoute.Price
        //                            });

        //                            transportList.Add(new TransportRoute
        //                            {
        //                                DepartureTime = transportRoute.ElapsedTime,
        //                                ElapsedTime = transportRoute.ElapsedTime.AddMinutes(10),
        //                                FromPoint = transportRoute.ToPoint,
        //                                ToPoint = toPoint,
        //                                TransportType = "OnFoot"
        //                            });

        //                            routeList.Add(new Route
        //                            {
        //                                Transport = transportList
        //                            });
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        return new QueryResult
        //        {
        //            DeparturePoint = fromPoint,
        //            ArrivalPoint = toPoint,
        //            Routes = routeList
        //        };
        //    }

        //    else throw new ArgumentException();

            return null;
        }



        private TransportRoute GetRouteSubStToSubSt(string stationFrom, string stationTo, DateTime queryDate)
        {
            return new TransportRoute
            {
                DepartureTime = queryDate,
                ElapsedTime = queryDate.AddMinutes(30),
                FromPoint = stationFrom,
                ToPoint = stationTo,
                Price = 50,
                TransportType = "Subway"
            };
        }


        public QueryResult GetFastestRoute(string fromPoint, string toPoint, DateTime queryDate)
        {
            var queryRoutesResult = GetRoute(fromPoint, toPoint, queryDate);
            if (queryRoutesResult.Routes.Count != 0)
            {
                TimeSpan elapsedTime = new TimeSpan();
                int id = 0;
                for (int i = 0; i < queryRoutesResult.Routes.Count; i++)
                {

                    if (elapsedTime == null || elapsedTime > queryRoutesResult.Routes[i].Transport[queryRoutesResult.Routes[i].Transport.Count - 1].ElapsedTime.Subtract(queryDate))
                    {
                        elapsedTime = queryRoutesResult.Routes[i].Transport[queryRoutesResult.Routes[i].Transport.Count - 1].ElapsedTime.Subtract(queryDate);
                        id = i;
                    }
                }
                ;
                List<TransportRoute> trRoute = new List<TransportRoute>();
                foreach (var item in queryRoutesResult.Routes[id].Transport)
                    trRoute.Add(item);
                List<Route> route = new List<Route>();
                route.Add(new Route
                {
                    Transport = trRoute
                });


                return new QueryResult
                {
                    DeparturePoint = fromPoint,
                    ArrivalPoint = toPoint,
                    Routes = route
                };
            }
            else throw new ArgumentNullException();
        }


        public string GetStationCode(string station)
        {
            if (context.LocalTrainStations.Any(s => s.Name.Equals(station)))
                return context.LocalTrainStations.Where(s => s.Name.Equals(station)).Select(s => s.Code).Single();

            else throw new ArgumentException();
        }


        public List<DubkiSchedule> GetDubkiSchedule(string from)
        {
            dayAbbreviation = dtfi.GetShortestDayName(DateTime.Now.DayOfWeek).ToUpper();
            if (context.DubkiBusesSchedule.Any(d => d.From.Equals(from)))
                return context.DubkiBusesSchedule.Where(d => d.From.Equals(from) && d.DayOfWeek.Any(w => w.Name == dayAbbreviation))
                    .Select(d => new DubkiSchedule
                    {
                        DepartureTime = d.DepartureTime,
                        From = d.From,
                        To = d.To
                    }).ToList();

            else throw new ArgumentException();
        }


        public List<string> GetAllBuildings()
        { 
            return context.Buildings.Select(b => b.Name).ToList();;
        }


        public List<TrainSchedule> GetTrainSchedule(string from, string to)
        {
            DateTime timeNow;
            timeNow = DateTime.Now;
            if (context.LocalTrainsSchedule.Count() != 0)
                if (context.LocalTrainStations.Any(s => s.Name.Equals(from)) && context.LocalTrainStations.Any(s => s.Name.Equals(to)))
                    return context.LocalTrainsSchedule.Where(s => s.DepartureStation.Name.Equals(from) && (s.DepartureTime.Hour > timeNow.Hour || s.DepartureTime.Hour == timeNow.Hour && s.DepartureTime.Minute >= timeNow.Minute) && s.Stops.Contains(context.LocalTrainStops.Single(t=> t.Station.Name.Equals(to))))
                       .Select(s => new TrainSchedule
                       {
                           DepartureStation = from,
                           DepartureTime = s.DepartureTime,
                           ArrivalStation = to,
                           ArrivalTime = s.Stops.Where(a => a.Station.Name.Equals(to)).Select(a => a.ArrivalTime).FirstOrDefault(),
                           Type = s.Type.Name,
                           Price = s.Type.Name.Equals("Suburban") ? context.LocalTrainPrices.Where(p => p.StationFrom.Name.Equals(from) && p.StationTo.Name.Equals(to)).Select(p => p.Price).FirstOrDefault() : context.LocalTrainPrices.Where(p => p.StationFrom.Name.Equals(from) && p.StationTo.Name.Equals(to)).Select(p => p.Price).FirstOrDefault() * 2
                       }).ToList();
                else throw new ArgumentException();
            else throw new NullReferenceException();
        }
    }
}
