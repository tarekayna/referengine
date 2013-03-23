namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableStoreAppScreenshot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreAppScreenshots",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 1024),
                        StoreAppInfoMsAppId = c.String(),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.Link);        }
        
        public override void Down()
        {
            DropTable("dbo.StoreAppScreenshots");
        }
    }
}
