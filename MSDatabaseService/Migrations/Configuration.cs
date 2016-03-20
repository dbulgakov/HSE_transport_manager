namespace MSDatabaseService.Migrations
{
    using HSE_transport_manager.Entities;
    using MSDatabaseService.Entities;
    using System;
    using System.Data.Entity;
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
                    Name = "Subway"
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
                        Type = context.TransportTypes.Single(t => t.Name.Equals(station.Type))
                    });
            context.SaveChanges();

            //Dormitories
            var dormData = LoadFromCSV.LoadDormitoryData("MSDatabaseService.Data.Dormitories.csv");
            foreach (var dorm in dormData)
                context.Dormitories.AddOrUpdate(
                    d => d.Address,
                    new Dormitory
                    {
                        Name = dorm.Name,
                        Region = dorm.Region,
                        City = dorm.City,
                        Address = dorm.Address,
                        Latitude = dorm.Latitude,
                        Longitude = dorm.Longitude,
                        SubwayStation = context.SubwayStations.Where(s => dorm.SubwayStation.Contains(s.Name)).ToList(),
                        CheckDubkiBus = dorm.CheckDubkiBus,
                        LocalTrainStation = context.LocalTrainStations.SingleOrDefault(s => s.Name == dorm.LocalTrainStation),
                        PublicTransport = context.PublicTransportSchedule.Where(t => dorm.PublicTransport.Contains(t.Number.ToString())).ToList()
                    });
            context.SaveChanges();

            //HSEBuildings
            var hseBuildings = LoadFromCSV.LoadHSEBuildingData("MSDatabaseService.Data.HSEBuildings.csv");
            foreach (var hse in hseBuildings)
                context.HSEBuildings.AddOrUpdate(
                    h => h.Address,
                    new HSEBuilding
                    {
                        Name = hse.Name,
                        Address = hse.Address,
                        Latitude = hse.Latitude,
                        Longitude = hse.Longitude,
                        SubwayStation = context.SubwayStations.Where(s => hse.SubwayStation.Contains(s.Name)).ToList()
                    });
            context.SaveChanges();
        
        }
    }
}
