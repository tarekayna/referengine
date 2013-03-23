namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppBackgroundImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "BackgroundImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "BackgroundImage");
        }
    }
}
