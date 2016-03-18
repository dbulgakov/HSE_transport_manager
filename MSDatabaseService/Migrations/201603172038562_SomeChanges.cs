namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChanges : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DubkiBusSchedules");
            CreateTable(
                "dbo.DayWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DubkiBusSchedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DubkiBusSchedules", t => t.DubkiBusSchedule_Id)
                .Index(t => t.DubkiBusSchedule_Id);
            
            AddColumn("dbo.Dormitories", "Name", c => c.String(nullable: false));
            AddColumn("dbo.DubkiBusSchedules", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.DubkiBusSchedules", "To", c => c.String(nullable: false));
            AddColumn("dbo.HSEBuildings", "Name", c => c.String(nullable: false));
            AddPrimaryKey("dbo.DubkiBusSchedules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DayWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules");
            DropIndex("dbo.DayWeeks", new[] { "DubkiBusSchedule_Id" });
            DropPrimaryKey("dbo.DubkiBusSchedules");
            DropColumn("dbo.HSEBuildings", "Name");
            DropColumn("dbo.DubkiBusSchedules", "To");
            DropColumn("dbo.DubkiBusSchedules", "Id");
            DropColumn("dbo.Dormitories", "Name");
            DropTable("dbo.DayWeeks");
            AddPrimaryKey("dbo.DubkiBusSchedules", "DepartureTime");
        }
    }
}
