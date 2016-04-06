namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChnages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubwayStations", "PublicTransport_Trip", "dbo.PublicTransports");
            DropIndex("dbo.SubwayStations", new[] { "PublicTransport_Trip" });
            CreateTable(
                "dbo.SubwayStationPublicTransports",
                c => new
                    {
                        SubwayStation_Id = c.Int(nullable: false),
                        PublicTransport_Trip = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubwayStation_Id, t.PublicTransport_Trip })
                .ForeignKey("dbo.SubwayStations", t => t.SubwayStation_Id, cascadeDelete: true)
                .ForeignKey("dbo.PublicTransports", t => t.PublicTransport_Trip, cascadeDelete: false)
                .Index(t => t.SubwayStation_Id)
                .Index(t => t.PublicTransport_Trip);
            
            DropColumn("dbo.SubwayStations", "PublicTransport_Trip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubwayStations", "PublicTransport_Trip", c => c.Int());
            DropForeignKey("dbo.SubwayStationPublicTransports", "PublicTransport_Trip", "dbo.PublicTransports");
            DropForeignKey("dbo.SubwayStationPublicTransports", "SubwayStation_Id", "dbo.SubwayStations");
            DropIndex("dbo.SubwayStationPublicTransports", new[] { "PublicTransport_Trip" });
            DropIndex("dbo.SubwayStationPublicTransports", new[] { "SubwayStation_Id" });
            DropTable("dbo.SubwayStationPublicTransports");
            CreateIndex("dbo.SubwayStations", "PublicTransport_Trip");
            AddForeignKey("dbo.SubwayStations", "PublicTransport_Trip", "dbo.PublicTransports", "Trip");
        }
    }
}
