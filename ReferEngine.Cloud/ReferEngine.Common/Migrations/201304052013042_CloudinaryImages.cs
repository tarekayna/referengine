namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CloudinaryImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CloudinaryImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        OriginalLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.WindowsAppStoreScreenshots");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WindowsAppStoreScreenshots",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 128),
                        StoreAppInfoMsAppId = c.String(),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.Link);
            
            DropTable("dbo.CloudinaryImages");
        }
    }
}
