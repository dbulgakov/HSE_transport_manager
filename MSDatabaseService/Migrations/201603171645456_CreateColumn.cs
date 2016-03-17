namespace MSDatabaseService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicTransportPrices", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LocalTrainPrices", "ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocalTrainPrices", "ModifiedDate");
            DropColumn("dbo.PublicTransportPrices", "ModifiedDate");
        }
    }
}
