namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreAppScreenshotKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StoreAppScreenshots", new[] { "ScreenshotLink", "StoreAppInfoMsAppId" });
            AddPrimaryKey("dbo.StoreAppScreenshots", "ScreenshotLink");
            AlterColumn("dbo.StoreAppScreenshots", "StoreAppInfoMsAppId", c => c.String());
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StoreAppScreenshots", new[] { "ScreenshotLink" });
            AddPrimaryKey("dbo.StoreAppScreenshots", new[] { "ScreenshotLink", "StoreAppInfoMsAppId" });
            AlterColumn("dbo.StoreAppScreenshots", "StoreAppInfoMsAppId", c => c.Long(nullable: false));
        }
    }
}
