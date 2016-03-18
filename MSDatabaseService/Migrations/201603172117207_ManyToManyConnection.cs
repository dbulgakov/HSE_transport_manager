namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyConnection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubwayStations", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.SubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropIndex("dbo.SubwayStations", new[] { "Dormitory_Id" });
            DropIndex("dbo.SubwayStations", new[] { "HSEBuilding_Id" });
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
            
            DropColumn("dbo.SubwayStations", "Dormitory_Id");
            DropColumn("dbo.SubwayStations", "HSEBuilding_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubwayStations", "HSEBuilding_Id", c => c.Int());
            AddColumn("dbo.SubwayStations", "Dormitory_Id", c => c.Int());
            DropForeignKey("dbo.HSEBuildingSubwayStations", "SubwayStation_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.HSEBuildingSubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings");
            DropForeignKey("dbo.SubwayStationDormitories", "Dormitory_Id", "dbo.Dormitories");
            DropForeignKey("dbo.SubwayStationDormitories", "SubwayStation_Id", "dbo.SubwayStations");
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "SubwayStation_Id" });
            DropIndex("dbo.HSEBuildingSubwayStations", new[] { "HSEBuilding_Id" });
            DropIndex("dbo.SubwayStationDormitories", new[] { "Dormitory_Id" });
            DropIndex("dbo.SubwayStationDormitories", new[] { "SubwayStation_Id" });
            DropTable("dbo.HSEBuildingSubwayStations");
            DropTable("dbo.SubwayStationDormitories");
            CreateIndex("dbo.SubwayStations", "HSEBuilding_Id");
            CreateIndex("dbo.SubwayStations", "Dormitory_Id");
            AddForeignKey("dbo.SubwayStations", "HSEBuilding_Id", "dbo.HSEBuildings", "Id");
            AddForeignKey("dbo.SubwayStations", "Dormitory_Id", "dbo.Dormitories", "Id");
        }
    }
}
