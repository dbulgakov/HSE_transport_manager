namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubwayElapsedTimes", "StationFrom_Id", "dbo.SubwayStations");
            DropForeignKey("dbo.SubwayElapsedTimes", "StationTo_Id", "dbo.SubwayStations");
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationFrom_Id" });
            DropIndex("dbo.SubwayElapsedTimes", new[] { "StationTo_Id" });
            DropTable("dbo.SubwayElapsedTimes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubwayElapsedTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElapsedTime = c.Int(nullable: false),
                        StationFrom_Id = c.Int(nullable: false),
                        StationTo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SubwayElapsedTimes", "StationTo_Id");
            CreateIndex("dbo.SubwayElapsedTimes", "StationFrom_Id");
            AddForeignKey("dbo.SubwayElapsedTimes", "StationTo_Id", "dbo.SubwayStations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SubwayElapsedTimes", "StationFrom_Id", "dbo.SubwayStations", "Id", cascadeDelete: true);
        }
    }
}
