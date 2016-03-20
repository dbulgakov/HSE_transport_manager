namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DayofWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DubkiBusSchedules",
                c => new
                    {
                        Trip = c.Int(nullable: false, identity: true),
                        DepartureTime = c.DateTime(nullable: false),
                        From = c.String(nullable: false),
                        To = c.String(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Trip)
                .ForeignKey("dbo.TransportTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.TransportTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PublicTransports",
                c => new
                    {
                        Trip = c.Int(nullable: false, identity: true),
                        DepartureTime = c.DateTime(nullable: false),
                        Number = c.Int(nullable: false),
                        From = c.String(nullable: false),
                        To = c.String(nullable: false),
                        Price_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Trip)
                .ForeignKey("dbo.PublicTransportPrices", t => t.Price_Id, cascadeDelete: true)
                .ForeignKey("dbo.TransportTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Price_Id)
                .Index(t => t.Type_Id);
            
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
                "dbo.SubwayStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.HSEBuildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PublicTransportPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocalTrainPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        StationFrom_Code = c.String(nullable: false, maxLength: 128),
                        StationTo_Code = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocalTrainStations", t => t.StationFrom_Code, cascadeDelete: false)
                .ForeignKey("dbo.LocalTrainStations", t => t.StationTo_Code, cascadeDelete: false)
                .Index(t => t.StationFrom_Code)
                .Index(t => t.StationTo_Code);
            
            CreateTable(
                "dbo.LocalTrainSchedules",
                c => new
                    {
                        DepartureTime = c.DateTime(nullable: false),
                        Uid = c.String(nullable: false),
                        ArrivalStation_Code = c.String(nullable: false, maxLength: 128),
                        DepartureStation_Code = c.String(nullable: false, maxLength: 128),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartureTime)
                .ForeignKey("dbo.LocalTrainStations", t => t.ArrivalStation_Code, cascadeDelete: false)
                .ForeignKey("dbo.LocalTrainStations", t => t.DepartureStation_Code, cascadeDelete: false)
                .ForeignKey("dbo.TransportTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.ArrivalStation_Code)
                .Index(t => t.DepartureStation_Code)
                .Index(t => t.Type_Id);
            
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
            
            CreateTable(
                "dbo.DubkiBusScheduleDayofWeeks",
                c => new
                    {
                        DubkiBusSchedule_Trip = c.Int(nullable: false),
                        DayofWeek_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DubkiBusSchedule_Trip, t.DayofWeek_Id })
                .ForeignKey("dbo.DubkiBusSchedules", t => t.DubkiBusSchedule_Trip, cascadeDelete: true)
                .ForeignKey("dbo.DayofWeeks", t => t.DayofWeek_Id, cascadeDelete: true)
                .Index(t => t.DubkiBusSchedule_Trip)
                .Index(t => t.DayofWeek_Id);
            
            CreateTable(
                "dbo.PublicTransportDayofWeeks",
                c => new
                    {
                        PublicTransport_Trip = c.Int(nullable: false),
                        DayofWeek_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PublicTransport_Trip, t.DayofWeek_Id })
                .ForeignKey("dbo.PublicTransports", t => t.PublicTransport_Trip, cascadeDelete: true)
                .ForeignKey("dbo.DayofWeeks", t => t.DayofWeek_Id, cascadeDelete: true)
                .Index(t => t.PublicTransport_Trip)
                .Index(t => t.DayofWeek_Id);
            
            CreateTable(
                "dbo.DormitoryPublicTransports",
                c => new
                    {
                        Dormitory_Id = c.Int(nullable: false),
                        PublicTransport_Trip = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dormitory_Id, t.PublicTransport_Trip })
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id, cascadeDelete: true)
                .ForeignKey("dbo.PublicTransports", t => t.PublicTransport_Trip, cascadeDelete: true)
                .Index(t => t.Dormitory_Id)
                .Index(t => t.PublicTransport_Trip);
            
            CreateTable(
                "dbo.SubwayStationDormitories",
                c => new
                    {
                        SubwayStation_Id = c.Int(nullable: false),
                        Dormitory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubwayStation_Id, t.Dormitory_Id })
                .ForeignKey("dbo.SubwayStations", t => t.SubwayStation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dormitories", t => t.Dormitory_Id, cascadeDelete: true)
                .Index(t => t.SubwayStation_Id)
                .Index(t => t.Dormitory_Id);
            
            CreateTable(
                "dbo.HSEBuildingSubwayStations",
                c => new
                    {
                        HSEBuilding_Id = c.Int(nullable: false),
                        SubwayStation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HSEBuilding_Id, t.SubwayStation_Id })
                .ForeignKey("dbo.HSEBuildings", t => t.HSEBuilding_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubwayStations", t => t.SubwayStation_Id, cascadeDelete: true)
                .Index(t => t.HSEBuilding_Id)
                .Index(t => t.SubwayStation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubwayElapsedTimes", "StationTo_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.SubwayElapsedTimes", "StationFrom_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.LocalTrainSchedules", "Type_Id", "dbo.TransportTypes");
            DropForeignKey("dbo.LocalTrainStops", "LocalTrainSchedule_DepartureTime", "dbo.LocalTrainSchedules");
            DropForeignKey("dbo.LocalTrainStops", "Station_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.LocalTrainSchedules", "DepartureStation_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.LocalTrainSchedules", "ArrivalStation_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.LocalTrainPrices", "StationTo_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.LocalTrainPrices", "StationFrom_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.PublicTransports", "Type_Id", "dbo.TransportTypes");
            DropForeignKey("dbo.PublicTransports", "Price_Id", "dbo.PublicTransportPrices");
            DropForeignKey("dbo.SubwayStations", "Type_Id", "dbo.TransportTypes");
            DropForeignKey("dbo.HSEBuildingSubwayStations", "SubwayStation_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.HSEBuildingSubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropForeignKey("dbo.SubwayStationDormitories", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.SubwayStationDormitories", "SubwayStation_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.DormitoryPublicTransports", "PublicTransport_Trip", "dbo.PublicTransports");
            DropForeignKey("dbo.DormitoryPublicTransports", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.Dormitories", "LocalTrainStation_Code", "dbo.LocalTrainStations");
            DropForeignKey("dbo.PublicTransportDayofWeeks", "DayofWeek_Id", "dbo.DayofWeeks");
            DropForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip", "dbo.PublicTransports");
            DropForeignKey("dbo.DubkiBusSchedules", "Type_Id", "dbo.TransportTypes");
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DayofWeek_Id", "dbo.DayofWeeks");
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Trip", "dbo.DubkiBusSchedules");
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "SubwayStation_Id" });
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "HSEBuilding_Id" });
            DropIndex("dbo.SubwayStationDormitories", new[] { "Dormitory_Id" });
            DropIndex("dbo.SubwayStationDormitories", new[] { "SubwayStation_Id" });
            DropIndex("dbo.DormitoryPublicTransports", new[] { "PublicTransport_Trip" });
            DropIndex("dbo.DormitoryPublicTransports", new[] { "Dormitory_Id" });
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "DayofWeek_Id" });
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_Trip" });
            DropIndex("dbo.DubkiBusScheduleDayofWeeks", new[] { "DayofWeek_Id" });
            DropIndex("dbo.DubkiBusScheduleDayofWeeks", new[] { "DubkiBusSchedule_Trip" });
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationTo_Id" });
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationFrom_Id" });
            DropIndex("dbo.LocalTrainStops", new[] { "LocalTrainSchedule_DepartureTime" });
            DropIndex("dbo.LocalTrainStops", new[] { "Station_Code" });
            DropIndex("dbo.LocalTrainSchedules", new[] { "Type_Id" });
            DropIndex("dbo.LocalTrainSchedules", new[] { "DepartureStation_Code" });
            DropIndex("dbo.LocalTrainSchedules", new[] { "ArrivalStation_Code" });
            DropIndex("dbo.LocalTrainPrices", new[] { "StationTo_Code" });
            DropIndex("dbo.LocalTrainPrices", new[] { "StationFrom_Code" });
            DropIndex("dbo.SubwayStations", new[] { "Type_Id" });
            DropIndex("dbo.Dormitories", new[] { "LocalTrainStation_Code" });
            DropIndex("dbo.PublicTransports", new[] { "Type_Id" });
            DropIndex("dbo.PublicTransports", new[] { "Price_Id" });
            DropIndex("dbo.DubkiBusSchedules", new[] { "Type_Id" });
            DropTable("dbo.HSEBuildingSubwayStations");
            DropTable("dbo.SubwayStationDormitories");
            DropTable("dbo.DormitoryPublicTransports");
            DropTable("dbo.PublicTransportDayofWeeks");
            DropTable("dbo.DubkiBusScheduleDayofWeeks");
            DropTable("dbo.SubwayElapsedTimes");
            DropTable("dbo.LocalTrainStops");
            DropTable("dbo.LocalTrainSchedules");
            DropTable("dbo.LocalTrainPrices");
            DropTable("dbo.PublicTransportPrices");
            DropTable("dbo.HSEBuildings");
            DropTable("dbo.SubwayStations");
            DropTable("dbo.LocalTrainStations");
            DropTable("dbo.Dormitories");
            DropTable("dbo.PublicTransports");
            DropTable("dbo.TransportTypes");
            DropTable("dbo.DubkiBusSchedules");
            DropTable("dbo.DayofWeeks");
        }
    }
}
