namespace MSDatabaseService.Migrations
{
    using MSDatabaseService.Entities;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MSDatabaseService.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MSDatabaseService.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            context.DayofWeek.AddOrUpdate(
                d => d.Name,
                new DayofWeek
                {
                    Name = "ПН"
                },
                new DayofWeek
                {
                    Name = "ВТ"
                },
                new DayofWeek
                {
                    Name = "СР"
                },
                new DayofWeek
                {
                    Name = "ЧТ"
                },
                new DayofWeek
                {
                    Name = "ПТ"
                },
                new DayofWeek
                {
                    Name = "СБ"
                },
                new DayofWeek
                {
                    Name = "ВС"
                });
            context.SaveChanges();
            //TransportType
            context.TransportTypes.AddOrUpdate(
                t => t.Name,
                new TransportType
                {
                    Name = "Suburban"
                },
                new TransportType
                {
                    Name = "ExpressSuburban"
                },
                new TransportType
                {
                    Name = "Bus"
                },
                new TransportType
                {
                    Name = "Tram"
                },
                new TransportType
                {
                    Name="Subway"
                });
            context.SaveChanges();

            //LocalTrainStations
            context.LocalTrainStations.AddOrUpdate(
                s => s.Name,
                new LocalTrainStation
                {
                    Code = "s9600721",
                    Name = "Одинцово"
                },
                new LocalTrainStation
                {
                    Code = "s9601728",
                    Name = "Кунцево"
                },
                new LocalTrainStation
                {
                    Code = "s9600821",
                    Name = "Фили"
                },
                new LocalTrainStation
                {
                    Code = "s9601666",
                    Name = "Беговая"
                },
                new LocalTrainStation
                {
                    Code = "s2000006",
                    Name = "Белорусский вокзал"
                });
            context.SaveChanges();


            //PublicTransportPrices
            context.PublicTransportPrices.AddOrUpdate(
                p => p.Price,
                new PublicTransportPrice
                {
                    Price = 50,
                    ModifiedDate = DateTime.Now
                },
                new PublicTransportPrice
                {
                    Price = 74,
                    ModifiedDate = DateTime.Now
                });
            context.SaveChanges();


            //PublicTransport
            var transportData = LoadFromCSV.LoadPublicTransportData("MSDatabaseService.Data.PublicTransport.csv");
            foreach (var vehicle in transportData)
                context.PublicTransportSchedule.AddOrUpdate(
                    v => v.Trip,
                    new PublicTransport
                    {
                        Trip = vehicle.Trip,
                        Number = vehicle.Number,
                        DayOfWeek = context.DayofWeek.Where(d=> vehicle.DayOfWeek.Contains(d.Name)).ToList(),
                        DepartureTime = vehicle.DepartureTime,
                        From = vehicle.From,
                        To = vehicle.To,
                        Type = context.TransportTypes.Single(t => t.Name.Equals(vehicle.Type)),
                        Price = context.PublicTransportPrices.Single(p => p.Price == 50 && vehicle.Type == "Tram" || p.Price == 74 && vehicle.Type == "Bus")
                    });
            context.SaveChanges();



            //DubkiBusSchedule
            var dubkiData = LoadFromCSV.LoadDubkiBusData("MSDatabaseService.Data.Dubki.csv");
            foreach (var bus in dubkiData)
                context.DubkiBusesSchedule.AddOrUpdate(
                    s => s.Trip,
                    new DubkiBusSchedule
                    {
                        Trip = bus.Trip,
                        DepartureTime = bus.DepartureTime,
                        From = bus.From,
                        DayOfWeek = context.DayofWeek.Where(d => bus.DayOfWeek.Contains(d.Name)).ToList(),
                        To = bus.To,
                        Type = context.TransportTypes.Single(t => t.Name == bus.Type)
                    });
            context.SaveChanges();


            //LocalTrainPrice
            context.LocalTrainPrices.AddOrUpdate(
                p => p.Id,
                new LocalTrainPrice
                {
                    StationFrom = context.LocalTrainStations.Single(s => s.Name == "Одинцово"),
                    StationTo = context.LocalTrainStations.Single(s => s.Name == "Кунцево"),
                    Price = 20.50,
                    ModifiedDate = DateTime.Now
                },
                new LocalTrainPrice
                {
                    StationFrom = context.LocalTrainStations.Single(s => s.Name == "Одинцово"),
                    StationTo = context.LocalTrainStations.Single(s => s.Name == "Фили"),
                    Price = 20.50,
                    ModifiedDate = DateTime.Now
                },
                new LocalTrainPrice
                {
                    StationFrom = context.LocalTrainStations.Single(s => s.Name == "Одинцово"),
                    StationTo = context.LocalTrainStations.Single(s => s.Name == "Беговая"),
                    Price = 41,
                    ModifiedDate = DateTime.Now
                },
                new LocalTrainPrice
                {
                    StationFrom = context.LocalTrainStations.Single(s => s.Name == "Одинцово"),
                    StationTo = context.LocalTrainStations.Single(s => s.Name == "Белорусский вокзал"),
                    Price = 61.50,
                    ModifiedDate = DateTime.Now
                });
            context.SaveChanges();

            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();
            //SubwayStations
            var subwayData = LoadFromCSV.LoadSubwayStationData("MSDatabaseService.Data.SubwayStations.csv");
            foreach (var station in subwayData)
                context.SubwayStations.AddOrUpdate(
                    s => s.Name,
                    new SubwayStation
                    {
                        Name = station.Name,
                        Latitude = station.Latitude,
                        Longitude = station.Longitude,
                        Type = context.TransportTypes.Single(t => t.Name.Equals(station.Type)),
                        PublicTransport = context.PublicTransportSchedule.Where(t => t.Number.Equals(station.BusNumber)).ToList()
                    });
            context.SaveChanges();

            //Buildings
            var buildData = LoadFromCSV.LoadBuildingData("MSDatabaseService.Data.Buildings.csv");
            foreach (var build in buildData)
            {
                if (build.TramStop!="")
                context.TramStop.AddOrUpdate(
                    t => t.Name,
                    new TramStop
                    {
                        Name = build.TramStop
                    });
                context.SaveChanges();

                context.Buildings.AddOrUpdate(
                    d => d.Address,
                    new Building
                    {
                        Name = build.Name,
                        Region = build.Region,
                        City = build.City,
                        Address = build.Address,
                        Latitude = build.Latitude,
                        Longitude = build.Longitude,
                        SubwayStation = context.SubwayStations.Where(s => build.SubwayStation.Contains(s.Name)).ToList(),
                        CheckDubkiBus = build.CheckDubkiBus,
                        LocalTrainStation = context.LocalTrainStations.SingleOrDefault(s => s.Name == build.LocalTrainStation),
                        PublicTransport = context.PublicTransportSchedule.Where(t => build.PublicTransport.Contains(t.Number.ToString())).ToList(),
                        TramStop=context.TramStop.SingleOrDefault(t=> t.Name==build.TramStop)
                    });
            }
            context.SaveChanges();
 
        }
    }
}
