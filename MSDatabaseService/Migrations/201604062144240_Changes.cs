namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Dormitories", newName: "Buildings");
            RenameTable(name: "dbo.PublicTransportDayofWeeks", newName: "DayofWeekPublicTransports");
            RenameTable(name: "dbo.DormitoryPublicTransports", newName: "PublicTransportBuildings");
            RenameTable(name: "dbo.SubwayStationDormitories", newName: "SubwayStationBuildings");
            DropForeignKey("dbo.HSEBuildingSubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropForeignKey("dbo.HSEBuildingSubwayStations", "SubwayStation_Id", "dbo.SubwayStations");
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "HSEBuilding_Id" });
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "SubwayStation_Id" });
            RenameColumn(table: "dbo.PublicTransportBuildings", name: "Dormitory_Id", newName: "Building_Id");
            RenameColumn(table: "dbo.SubwayStationBuildings", name: "Dormitory_Id", newName: "Building_Id");
            RenameIndex(table: "dbo.PublicTransportBuildings", name: "IX_Dormitory_Id", newName: "IX_Building_Id");
            RenameIndex(table: "dbo.SubwayStationBuildings", name: "IX_Dormitory_Id", newName: "IX_Building_Id");
            DropPrimaryKey("dbo.DayofWeekPublicTransports");
            DropPrimaryKey("dbo.PublicTransportBuildings");
            CreateTable(
                "dbo.TramStops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Buildings", "TramStop_Id", c => c.Int());
            AddColumn("dbo.SubwayStations", "PublicTransport_Trip", c => c.Int());
            AddPrimaryKey("dbo.DayofWeekPublicTransports", new[] { "DayofWeek_Id", "PublicTransport_Trip" });
            AddPrimaryKey("dbo.PublicTransportBuildings", new[] { "PublicTransport_Trip", "Building_Id" });
            CreateIndex("dbo.Buildings", "TramStop_Id");
            CreateIndex("dbo.SubwayStations", "PublicTransport_Trip");
            AddForeignKey("dbo.SubwayStations", "PublicTransport_Trip", "dbo.PublicTransports", "Trip");
            AddForeignKey("dbo.Buildings", "TramStop_Id", "dbo.TramStops", "Id");
            DropTable("dbo.HSEBuildings");
            DropTable("dbo.HSEBuildingSubwayStations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HSEBuildingSubwayStations",
                c => new
                    {
                        HSEBuilding_Id = c.Int(nullable: false),
                        SubwayStation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HSEBuilding_Id, t.SubwayStation_Id });
            
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
            
            DropForeignKey("dbo.Buildings", "TramStop_Id", "dbo.TramStops");
            DropForeignKey("dbo.SubwayStations", "PublicTransport_Trip", "dbo.PublicTransports");
            DropIndex("dbo.SubwayStations", new[] { "PublicTransport_Trip" });
            DropIndex("dbo.Buildings", new[] { "TramStop_Id" });
            DropPrimaryKey("dbo.PublicTransportBuildings");
            DropPrimaryKey("dbo.DayofWeekPublicTransports");
            DropColumn("dbo.SubwayStations", "PublicTransport_Trip");
            DropColumn("dbo.Buildings", "TramStop_Id");
            DropTable("dbo.TramStops");
            AddPrimaryKey("dbo.PublicTransportBuildings", new[] { "Dormitory_Id", "PublicTransport_Trip" });
            AddPrimaryKey("dbo.DayofWeekPublicTransports", new[] { "PublicTransport_Trip", "DayofWeek_Id" });
            RenameIndex(table: "dbo.SubwayStationBuildings", name: "IX_Building_Id", newName: "IX_Dormitory_Id");
            RenameIndex(table: "dbo.PublicTransportBuildings", name: "IX_Building_Id", newName: "IX_Dormitory_Id");
            RenameColumn(table: "dbo.SubwayStationBuildings", name: "Building_Id", newName: "Dormitory_Id");
            RenameColumn(table: "dbo.PublicTransportBuildings", name: "Building_Id", newName: "Dormitory_Id");
            CreateIndex("dbo.HSEBuildingSubwayStations", "SubwayStation_Id");
            CreateIndex("dbo.HSEBuildingSubwayStations", "HSEBuilding_Id");
            AddForeignKey("dbo.HSEBuildingSubwayStations", "SubwayStation_Id", "dbo.SubwayStations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HSEBuildingSubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.SubwayStationBuildings", newName: "SubwayStationDormitories");
            RenameTable(name: "dbo.PublicTransportBuildings", newName: "DormitoryPublicTransports");
            RenameTable(name: "dbo.DayofWeekPublicTransports", newName: "PublicTransportDayofWeeks");
            RenameTable(name: "dbo.Buildings", newName: "Dormitories");
        }
    }
}
