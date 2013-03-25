namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreAppInfoColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreAppInfoes", "BackgroundColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreAppInfoes", "BackgroundColor");
        }
    }
}
