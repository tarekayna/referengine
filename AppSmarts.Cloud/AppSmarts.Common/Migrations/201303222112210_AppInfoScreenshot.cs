using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppInfoScreenshot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreAppScreenshots",
                c => new
                    {
                        ScreenshotLink = c.String(nullable: false, maxLength: 1024),
                        StoreAppInfoMsAppId = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => new { t.ScreenshotLink, t.StoreAppInfoMsAppId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoreAppScreenshots");
        }
    }
}
