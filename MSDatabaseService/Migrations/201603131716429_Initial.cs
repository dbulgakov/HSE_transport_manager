namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        Destination_Id = c.Int(nullable: false),
                        Price_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartureTime)
                .ForeignKey("dbo.SubwayStations", t => t.Destination_Id, cascadeDelete: true)
                .ForeignKey("dbo.BusPrices", t => t.Price_Id, cascadeDelete: true)
                .Index(t => t.Destination_Id)
                .Index(t => t.Price_Id);
            
            CreateTable(
                "dbo.SubwayStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Dormitory_Id = c.Int(),
                        HSEBuilding_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id)
                .ForeignKey("dbo.HSEBuildings", t => t.HSEBuilding_Id)
                .Index(t => t.Dormitory_Id)
                .Index(t => t.HSEBuilding_Id);
            
            CreateTable(
                "dbo.BusPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dormitories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Region = c.String(),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        LocalTrainStation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStations", t => t.LocalTrainStation_Id)
                .Index(t => t.LocalTrainStation_Id);
            
            CreateTable(
                "dbo.LocalTrainStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TramSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        Stop = c.String(nullable: false),
                        Price_Id = c.Int(nullable: false),
                        Dormitory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.DepartureTime)
                .ForeignKey("dbo.TramPrices", t => t.Price_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id)
                .Index(t => t.Price_Id)
                .Index(t => t.Dormitory_Id);
            
            CreateTable(
                "dbo.TramPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DubkiBusSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DepartureTime);
            
            CreateTable(
                "dbo.HSEBuildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocalTrainPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        Station_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStops", t => t.Station_Id, cascadeDelete: true)
                .Index(t => t.Station_Id);
            
            CreateTable(
                "dbo.LocalTrainStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArrivalTime = c.DateTime(nullable: false),
                        ElapsedTime = c.DateTime(nullable: false),
                        Station_Id = c.Int(nullable: false),
                        LocalTrainSchedule_DepartureTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStations", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.LocalTrainSchedules", t => t.LocalTrainSchedule_DepartureTime)
                .Index(t => t.Station_Id)
                .Index(t => t.LocalTrainSchedule_DepartureTime);
            
            CreateTable(
                "dbo.LocalTrainSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Uid = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DepartureTime);
            
            CreateTable(
                "dbo.SubwayElapsedTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElapsedTime = c.DateTime(nullable: false),
                        StationFrom_Id = c.Int(nullable: false),
                        StationTo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubwayStations", t => t.StationFrom_Id, cascadeDelete: false)
                .ForeignKey("dbo.SubwayStations", t => t.StationTo_Id, cascadeDelete: false)
                .Index(t => t.StationFrom_Id)
                .Index(t => t.StationTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubwayElapsedTimes", "StationTo_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.SubwayElapsedTimes", "StationFrom_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.LocalTrainStops", "LocalTrainSchedule_DepartureTime", "dbo.LocalTrainSchedules");
            DropForeignKey("dbo.LocalTrainPrices", "Station_Id", "dbo.LocalTrainStops");
            DropForeignKey("dbo.LocalTrainStops", "Station_Id", "dbo.LocalTrainStations");
            DropForeignKey("dbo.SubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropForeignKey("dbo.TramSchedules", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.TramSchedules", "Price_Id", "dbo.TramPrices");
            DropForeignKey("dbo.SubwayStations", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.Dormitories", "LocalTrainStation_Id", "dbo.LocalTrainStations");
            DropForeignKey("dbo.BusSchedules", "Price_Id", "dbo.BusPrices");
            DropForeignKey("dbo.BusSchedules", "Destination_Id", "dbo.SubwayStations");
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationTo_Id" });
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationFrom_Id" });
            DropIndex("dbo.LocalTrainStops", new[] { "LocalTrainSchedule_DepartureTime" });
            DropIndex("dbo.LocalTrainStops", new[] { "Station_Id" });
            DropIndex("dbo.LocalTrainPrices", new[] { "Station_Id" });
            DropIndex("dbo.TramSchedules", new[] { "Dormitory_Id" });
            DropIndex("dbo.TramSchedules", new[] { "Price_Id" });
            DropIndex("dbo.Dormitories", new[] { "LocalTrainStation_Id" });
            DropIndex("dbo.SubwayStations", new[] { "HSEBuilding_Id" });
            DropIndex("dbo.SubwayStations", new[] { "Dormitory_Id" });
            DropIndex("dbo.BusSchedules", new[] { "Price_Id" });
            DropIndex("dbo.BusSchedules", new[] { "Destination_Id" });
            DropTable("dbo.SubwayElapsedTimes");
            DropTable("dbo.LocalTrainSchedules");
            DropTable("dbo.LocalTrainStops");
            DropTable("dbo.LocalTrainPrices");
            DropTable("dbo.HSEBuildings");
            DropTable("dbo.DubkiBusSchedules");
            DropTable("dbo.TramPrices");
            DropTable("dbo.TramSchedules");
            DropTable("dbo.LocalTrainStations");
            DropTable("dbo.Dormitories");
            DropTable("dbo.BusPrices");
            DropTable("dbo.SubwayStations");
            DropTable("dbo.BusSchedules");
        }
    }
}
