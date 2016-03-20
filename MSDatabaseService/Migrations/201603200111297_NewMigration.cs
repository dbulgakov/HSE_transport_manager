namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules");
            DropForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_DepartureTime", "dbo.PublicTransports");
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_DepartureTime" });
            RenameColumn(table: "dbo.DubkiBusScheduleDayofWeeks", name: "DubkiBusSchedule_Id", newName: "DubkiBusSchedule_Trip");
            RenameColumn(table: "dbo.PublicTransportDayofWeeks", name: "PublicTransport_DepartureTime", newName: "PublicTransport_Trip");
            RenameIndex(table: "dbo.DubkiBusScheduleDayofWeeks", name: "IX_DubkiBusSchedule_Id", newName: "IX_DubkiBusSchedule_Trip");
            DropPrimaryKey("dbo.DubkiBusSchedules");
            DropPrimaryKey("dbo.PublicTransports");
            DropPrimaryKey("dbo.PublicTransportDayofWeeks");
            AddColumn("dbo.PublicTransports", "Trip", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DubkiBusSchedules", "Trip", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DubkiBusSchedules", "Trip");
            AddPrimaryKey("dbo.PublicTransports", "Trip");
            AddPrimaryKey("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_Trip", "DayofWeek_Id" });
            CreateIndex("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip");
            AddForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Trip", "dbo.DubkiBusSchedules", "Trip", cascadeDelete: true);
            AddForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip", "dbo.PublicTransports", "Trip", cascadeDelete: true);
            DropColumn("dbo.DubkiBusSchedules", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DubkiBusSchedules", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip", "dbo.PublicTransports");
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Trip", "dbo.DubkiBusSchedules");
            DropIndex("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_Trip" });
            DropPrimaryKey("dbo.PublicTransportDayofWeeks");
            DropPrimaryKey("dbo.PublicTransports");
            DropPrimaryKey("dbo.DubkiBusSchedules");
            AlterColumn("dbo.PublicTransportDayofWeeks", "PublicTransport_Trip", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DubkiBusSchedules", "Trip", c => c.Int(nullable: false));
            DropColumn("dbo.PublicTransports", "Trip");
            AddPrimaryKey("dbo.PublicTransportDayofWeeks", new[] { "PublicTransport_DepartureTime", "DayofWeek_Id" });
            AddPrimaryKey("dbo.PublicTransports", "DepartureTime");
            AddPrimaryKey("dbo.DubkiBusSchedules", "Id");
            RenameIndex(table: "dbo.DubkiBusScheduleDayofWeeks", name: "IX_DubkiBusSchedule_Trip", newName: "IX_DubkiBusSchedule_Id");
            RenameColumn(table: "dbo.PublicTransportDayofWeeks", name: "PublicTransport_Trip", newName: "PublicTransport_DepartureTime");
            RenameColumn(table: "dbo.DubkiBusScheduleDayofWeeks", name: "DubkiBusSchedule_Trip", newName: "DubkiBusSchedule_Id");
            CreateIndex("dbo.PublicTransportDayofWeeks", "PublicTransport_DepartureTime");
            AddForeignKey("dbo.PublicTransportDayofWeeks", "PublicTransport_DepartureTime", "dbo.PublicTransports", "DepartureTime", cascadeDelete: true);
            AddForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules", "Id", cascadeDelete: true);
        }
    }
}
