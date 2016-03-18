namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDubkiBusScheduleEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DayWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules");
            DropIndex("dbo.DayWeeks", new[] { "DubkiBusSchedule_Id" });
            CreateTable(
                "dbo.DayofWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DubkiBusScheduleDayofWeeks",
                c => new
                    {
                        DubkiBusSchedule_Id = c.Int(nullable: false),
                        DayofWeek_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DubkiBusSchedule_Id, t.DayofWeek_Id })
                .ForeignKey("dbo.DubkiBusSchedules", t => t.DubkiBusSchedule_Id, cascadeDelete: true)
                .ForeignKey("dbo.DayofWeeks", t => t.DayofWeek_Id, cascadeDelete: true)
                .Index(t => t.DubkiBusSchedule_Id)
                .Index(t => t.DayofWeek_Id);
            
            DropTable("dbo.DayWeeks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DayWeeks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DubkiBusSchedule_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DayofWeek_Id", "dbo.DayofWeeks");
            DropForeignKey("dbo.DubkiBusScheduleDayofWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules");
            DropIndex("dbo.DubkiBusScheduleDayofWeeks", new[] { "DayofWeek_Id" });
            DropIndex("dbo.DubkiBusScheduleDayofWeeks", new[] { "DubkiBusSchedule_Id" });
            DropTable("dbo.DubkiBusScheduleDayofWeeks");
            DropTable("dbo.DayofWeeks");
            CreateIndex("dbo.DayWeeks", "DubkiBusSchedule_Id");
            AddForeignKey("dbo.DayWeeks", "DubkiBusSchedule_Id", "dbo.DubkiBusSchedules", "Id");
        }
    }
}
