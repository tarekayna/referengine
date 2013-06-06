using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class DropTableStoreAppScreenshots : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.StoreAppScreenshots");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StoreAppScreenshots",
                c => new
                    {
                        ScreenshotLink = c.String(nullable: false, maxLength: 128),
                        StoreAppInfoMsAppId = c.String(),
                    })
                .PrimaryKey(t => t.ScreenshotLink);
            
        }
    }
}
