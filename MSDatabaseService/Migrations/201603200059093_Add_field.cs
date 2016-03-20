namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_field : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PublicTransportDayofWeeks",
                c => new
                    {
                        PublicTransport_DepartureTime = c.DateTime(nullable: false),
                        DayofWeek_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PublicTransport_DepartureTime, t.DayofWeek_Id })
                .ForeignKey("dbo.PublicTransports", t => t.PublicTransport_DepartureTime, cascadeDelete: true)
                .ForeignKey("dbo.DayofWeeks", t => t.DayofWeek_Id, cascadeDelete: true)
                .Index(t => t.PublicTransport_DepartureTime)
                .Index(t => t.DayofWeek_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublicTransportDayofWeeks", "DayofWeek_Id", "dbo.DayofWeeks");
            DropForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_DepartureTime", "dbo.PublicTransports");
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "DayofWeek_Id" });
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_DepartureTime" });
            DropTable("dbo.PublicTransportDayofWeeks");
        }
    }
}
