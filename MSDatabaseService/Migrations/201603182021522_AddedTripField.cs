namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTripField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DubkiBusSchedules", "Trip", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DubkiBusSchedules", "Trip");
        }
    }
}
