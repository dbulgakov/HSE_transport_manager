namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocalTrainSchedules", "ArrivalStation_Code", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.LocalTrainSchedules", "ArrivalStation_Code");
            AddForeignKey("dbo.LocalTrainSchedules", "ArrivalStation_Code", "dbo.LocalTrainStations", "Code", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocalTrainSchedules", "ArrivalStation_Code", "dbo.LocalTrainStations");
            DropIndex("dbo.LocalTrainSchedules", new[] { "ArrivalStation_Code" });
            DropColumn("dbo.LocalTrainSchedules", "ArrivalStation_Code");
        }
    }
}
