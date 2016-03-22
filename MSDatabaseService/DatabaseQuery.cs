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
        private List<Route> routeList;
        private List<TransportRoute> transportList;
        private bool check;
        private List<Coordinate> coordinate;
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
                        stopsListArrival.Add(stopsList[stopsList.Count-1].ArrivalTime);
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



        public List<Coordinate> GetCoordinates(string fromPoint, string toPoint)
        {
            coordinate = new List<Coordinate>();
            var c = context.Dormitories.Where(d => d.Name.Equals(fromPoint)).Select(d => new Coordinate
                {
                    Latitude = d.Latitude,
                    Longitude = d.Longitude
                }).Single();
            coordinate.Add(c);

            c = context.HSEBuildings.Where(b => b.Name.Equals(toPoint)).Select(b => new Coordinate
            {
                Latitude = b.Latitude,
                Longitude = b.Longitude
            }).Single();
            coordinate.Add(c);

            return coordinate;
        }



        public QueryResult GetRoute(string fromPoint, string toPoint, DateTime queryDate)
        {
            dayAbbreviation = dtfi.GetShortestDayName(queryDate.DayOfWeek).ToUpper();
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

                minutes = 20;
                
                if (queryDate.Minute + 20 > 60)
                    {
                        hours++;
                        minutes = queryDate.Minute - 40;
                    }

                //PublicTransport 
                if (context.PublicTransportSchedule.Where(t => t.Dormitory.Any(d => d.Name == fromPoint) == true).Select(t => t.DepartureTime).Count() != 0 && context.PublicTransportSchedule.Where(p => p.Dormitory.Any(d => d.Name == fromPoint) == true && p.DayOfWeek.Any(d => d.Name == dayAbbreviation) == true && (p.DepartureTime.Hour > queryDate.Hour + hours || p.DepartureTime.Hour == queryDate.Hour + hours && p.DepartureTime.Minute >= queryDate.Minute + minutes))
                                             .Select(t => new
                                                          {
                                                              From = t.From,
                                                              To = t.To,
                                                              Number = t.Number,
                                                              DepartureTime = t.DepartureTime,
                                                              TransportType = context.TransportTypes.Where(y => y.Id == t.Type.Id).Select(y => y.Name).FirstOrDefault(),
                                                              Price = t.Price.Price
                                                          })
                                             .First()!=null)

                {
                    
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

                    // LocalTrain - Works
                    var localStation = context.Dormitories.Where(r => r.Name == fromPoint)
                                                              .Select(r => new
                                                              {
                                                                 StationName = r.LocalTrainStation.Name,
                                                                 Code = r.LocalTrainStation.Code
                                                              }).Single();

                    if (localStation != null)
                    {
                        minutes = 20;
                        if (check)
                        {
                            fromPoint = "Автовокзал";
                            minutes = 65;
                        }

                        if (queryDate.Minute + minutes > 60)
                        {
                            hours++;
                            minutes = queryDate.Minute - 40;
                        }

                        var localStationsQuery = context.LocalTrainsSchedule.Where(s => s.DepartureStation.Name == localStation.StationName && (s.DepartureTime.Hour > queryDate.Hour + hours || s.DepartureTime.Hour == queryDate.Hour + hours && s.DepartureTime.Minute >= queryDate.Minute + minutes)).Select(r => new
                             {
                                 DepartureTime = r.DepartureTime,
                                 Stops = r.Stops.Select(t => new
                                 {
                                     ArrivalTime=t.ArrivalTime,
                                     ElapsedTime=t.ElapsedTime,
                                     StationName=t.Station.Name,
                                     Code=t.Station.Code
                                 }).ToList(),
                                 Type = r.Type.Name
                             }).First();

                        foreach (var stationHSE in subwayStationHSE)
                        {
                            transportList = new List<TransportRoute>();
                            foreach (var stationStop in localStationsQuery.Stops)
                            {
                                transportList = new List<TransportRoute>();
                                transportList.Add(new TransportRoute
                                {
                                    DepartureTime = queryDate.AddMinutes(minutes-10),
                                    ElapsedTime = queryDate.AddMinutes(minutes),
                                    FromPoint = fromPoint,
                                    ToPoint = localStation.StationName,
                                    TransportType = "OnFoot"
                                });

                                transportList.Add(new TransportRoute
                                {
                                    DepartureTime = localStationsQuery.DepartureTime,
                                    ElapsedTime = stationStop.ElapsedTime,
                                    FromPoint = localStation.StationName,
                                    ToPoint = stationStop.StationName,
                                    TransportType = localStationsQuery.Type,
                                    Price = localStationsQuery.Type=="Suburban" ? context.LocalTrainPrices.Where(p => p.StationFrom.Code == localStation.Code && p.StationTo.Code == stationStop.Code).Select(p => p.Price).Single() : context.LocalTrainPrices.Where(p => p.StationFrom.Code == localStation.Code && p.StationTo.Code == stationStop.Code).Select(p => p.Price).Single()*2
                                });

                                stationFix = stationStop.StationName;
                                if (stationStop.StationName == "Белорусский вокзал") stationFix = "Белорусская";
                                if (stationStop.StationName == "Кунцево") stationFix = "Кунцевская";

                                transportRoute = GetRouteSubStToSubSt(stationFix, stationHSE, queryDate.AddMinutes(minutes + 10));

                                transportList.Add(new TransportRoute
                                {
                                    DepartureTime = queryDate.AddMinutes(minutes + 10),
                                    ElapsedTime = transportRoute.ElapsedTime,
                                    FromPoint = transportRoute.FromPoint,
                                    ToPoint = transportRoute.ToPoint,
                                    TransportType = transportRoute.TransportType
                                });

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
                }

            return new QueryResult
            {
                DeparturePoint=fromPoint,
                ArrivalPoint=toPoint,
                Routes=routeList
            };
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


        public QueryResult GetFastestRoute(string fromPoint, string toPoint, DateTime queryDate)
        {
            var queryRoutesResult = GetRoute(fromPoint, toPoint, queryDate);
            TimeSpan elapsedTime= new TimeSpan();
            int id=0;
            for( int i= 0; i<queryRoutesResult.Routes.Count; i++)
            {
                
                if (elapsedTime==null || elapsedTime > queryRoutesResult.Routes[i].Transport[queryRoutesResult.Routes[i].Transport.Count-1].ElapsedTime.Subtract(queryDate))
                {
                    elapsedTime=queryRoutesResult.Routes[i].Transport[queryRoutesResult.Routes[i].Transport.Count-1].ElapsedTime.Subtract(queryDate);
                    id = i;
                }
            }
            ;
            List<TransportRoute> trRoute = new List<TransportRoute>();
            foreach(var item in queryRoutesResult.Routes[id].Transport)
                trRoute.Add(item);
            List<Route> route = new List<Route>();
            route.Add(new Route
                {
                    Transport = trRoute
                });
            return new QueryResult
                {
                    DeparturePoint=fromPoint,
                    ArrivalPoint=toPoint,
                    Routes= route
                };
        }
    }
}
