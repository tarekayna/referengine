using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppCloudinaryImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "LogoImage_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Apps", "HighQualityLogoImage_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Apps", "BackgroundImage_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.WindowsAppStoreInfoes", "LogoImage_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Apps", "LogoImage_Id");
            CreateIndex("dbo.Apps", "HighQualityLogoImage_Id");
            CreateIndex("dbo.Apps", "BackgroundImage_Id");
            CreateIndex("dbo.WindowsAppStoreInfoes", "LogoImage_Id");
            AddForeignKey("dbo.Apps", "LogoImage_Id", "dbo.CloudinaryImages", "Id");
            AddForeignKey("dbo.Apps", "HighQualityLogoImage_Id", "dbo.CloudinaryImages", "Id");
            AddForeignKey("dbo.Apps", "BackgroundImage_Id", "dbo.CloudinaryImages", "Id");
            AddForeignKey("dbo.WindowsAppStoreInfoes", "LogoImage_Id", "dbo.CloudinaryImages", "Id");
            DropColumn("dbo.Apps", "LogoLink50");
            DropColumn("dbo.Apps", "LogoLinkHighQuality");
            DropColumn("dbo.Apps", "BackgroundImage");
            DropColumn("dbo.WindowsAppStoreInfoes", "LogoLink");
            DropTable("dbo.AppScreenshots");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppScreenshots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AppId = c.Long(nullable: false),
                        Title = c.String(),
                        Description = c.String(nullable: false),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WindowsAppStoreInfoes", "LogoLink", c => c.String());
            AddColumn("dbo.Apps", "BackgroundImage", c => c.String());
            AddColumn("dbo.Apps", "LogoLinkHighQuality", c => c.String());
            AddColumn("dbo.Apps", "LogoLink50", c => c.String(nullable: false));
            DropForeignKey("dbo.WindowsAppStoreInfoes", "LogoImage_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.Apps", "BackgroundImage_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.Apps", "HighQualityLogoImage_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.Apps", "LogoImage_Id", "dbo.CloudinaryImages");
            DropIndex("dbo.WindowsAppStoreInfoes", new[] { "LogoImage_Id" });
            DropIndex("dbo.Apps", new[] { "BackgroundImage_Id" });
            DropIndex("dbo.Apps", new[] { "HighQualityLogoImage_Id" });
            DropIndex("dbo.Apps", new[] { "LogoImage_Id" });
            DropColumn("dbo.WindowsAppStoreInfoes", "LogoImage_Id");
            DropColumn("dbo.Apps", "BackgroundImage_Id");
            DropColumn("dbo.Apps", "HighQualityLogoImage_Id");
            DropColumn("dbo.Apps", "LogoImage_Id");
        }
    }
}
