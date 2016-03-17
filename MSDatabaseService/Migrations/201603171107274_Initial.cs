namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PublicTransports",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        From = c.String(nullable: false),
                        Price_Id = c.Int(nullable: false),
                        SubwayStation_Id = c.Int(nullable: false),
                        Dormitory_Id = c.Int(),
                        Dormitory_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.DepartureTime)
                .ForeignKey("dbo.PublicTransportPrices", t => t.Price_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubwayStations", t => t.SubwayStation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id)
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id1)
                .Index(t => t.Price_Id)
                .Index(t => t.SubwayStation_Id)
                .Index(t => t.Dormitory_Id)
                .Index(t => t.Dormitory_Id1);
            
            CreateTable(
                "dbo.PublicTransportPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Dormitories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Region = c.String(),
                        City = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        CheckDubkiBus = c.Boolean(nullable: false),
                        LocalTrainStation_Code = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStations", t => t.LocalTrainStation_Code)
                .Index(t => t.LocalTrainStation_Code);
            
            CreateTable(
                "dbo.LocalTrainStations",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DubkiBusSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        From = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DepartureTime);
            
            CreateTable(
                "dbo.HSEBuildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        StationFrom_Id = c.Int(nullable: false),
                        StationTo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStops", t => t.StationFrom_Id, cascadeDelete: true)
                .ForeignKey("dbo.LocalTrainStops", t => t.StationTo_Id, cascadeDelete: true)
                .Index(t => t.StationFrom_Id)
                .Index(t => t.StationTo_Id);
            
            CreateTable(
                "dbo.LocalTrainStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArrivalTime = c.DateTime(nullable: false),
                        ElapsedTime = c.DateTime(nullable: false),
                        Station_Code = c.String(nullable: false, maxLength: 128),
                        LocalTrainSchedule_DepartureTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStations", t => t.Station_Code, cascadeDelete: true)
                .ForeignKey("dbo.LocalTrainSchedules", t => t.LocalTrainSchedule_DepartureTime)
                .Index(t => t.Station_Code)
                .Index(t => t.LocalTrainSchedule_DepartureTime);
            
            CreateTable(
                "dbo.LocalTrainSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Uid = c.String(nullable: false),
                        DepartureStation_Code = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.DepartureTime)
                .ForeignKey("dbo.LocalTrainStations", t => t.DepartureStation_Code, cascadeDelete: true)
                .Index(t => t.DepartureStation_Code);
            
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
                .ForeignKey("dbo.SubwayStations", t => t.StationFrom_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubwayStations", t => t.StationTo_Id, cascadeDelete: true)
                .Index(t => t.StationFrom_Id)
                .Index(t => t.StationTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubwayElapsedTimes", "StationTo_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.SubwayElapsedTimes", "StationFrom_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.LocalTrainStops", "LocalTrainSchedule_DepartureTime", "dbo.LocalTrainSchedules");
            DropForeignKey("dbo.LocalTrainSchedules", "DepartureStation_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.LocalTrainPrices", "StationTo_Id", "dbo.LocalTrainStops");
            DropForeignKey("dbo.LocalTrainPrices", "StationFrom_Id", "dbo.LocalTrainStops");
            DropForeignKey("dbo.LocalTrainStops", "Station_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.SubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropForeignKey("dbo.PublicTransports", "Dormitory_Id1", "dbo.Dormitories");
            DropForeignKey("dbo.SubwayStations", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.Dormitories", "LocalTrainStation_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.PublicTransports", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.PublicTransports", "SubwayStation_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.PublicTransports", "Price_Id", "dbo.PublicTransportPrices");
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationTo_Id" });
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationFrom_Id" });
            DropIndex("dbo.LocalTrainSchedules", new[] { "DepartureStation_Code" });
            DropIndex("dbo.LocalTrainStops", new[] { "LocalTrainSchedule_DepartureTime" });
            DropIndex("dbo.LocalTrainStops", new[] { "Station_Code" });
            DropIndex("dbo.LocalTrainPrices", new[] { "StationTo_Id" });
            DropIndex("dbo.LocalTrainPrices", new[] { "StationFrom_Id" });
            DropIndex("dbo.Dormitories", new[] { "LocalTrainStation_Code" });
            DropIndex("dbo.SubwayStations", new[] { "HSEBuilding_Id" });
            DropIndex("dbo.SubwayStations", new[] { "Dormitory_Id" });
            DropIndex("dbo.PublicTransports", new[] { "Dormitory_Id1" });
            DropIndex("dbo.PublicTransports", new[] { "Dormitory_Id" });
            DropIndex("dbo.PublicTransports", new[] { "SubwayStation_Id" });
            DropIndex("dbo.PublicTransports", new[] { "Price_Id" });
            DropTable("dbo.SubwayElapsedTimes");
            DropTable("dbo.LocalTrainSchedules");
            DropTable("dbo.LocalTrainStops");
            DropTable("dbo.LocalTrainPrices");
            DropTable("dbo.HSEBuildings");
            DropTable("dbo.DubkiBusSchedules");
            DropTable("dbo.LocalTrainStations");
            DropTable("dbo.Dormitories");
            DropTable("dbo.SubwayStations");
            DropTable("dbo.PublicTransportPrices");
            DropTable("dbo.PublicTransports");
        }
    }
}
