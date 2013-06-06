using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class WindowsAppStoreInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WindowsAppStoreLinks",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Link);
            
            CreateTable(
                "dbo.WindowsAppStoreInfoes",
                c => new
                    {
                        MsAppId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Rating = c.Double(nullable: false),
                        NumberOfRatings = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Category = c.String(),
                        AgeRating = c.String(),
                        Developer = c.String(),
                        Copyright = c.String(),
                        LogoLink = c.String(),
                        DescriptionHtml = c.String(),
                        FeaturesHtml = c.String(),
                        WebsiteLink = c.String(),
                        SupportLink = c.String(),
                        PrivacyPolicyLink = c.String(),
                        ReleaseNotes = c.String(),
                        Architecture = c.String(),
                        Languages = c.String(),
                        PackageFamilyName = c.String(),
                        AppStoreLink = c.String(),
                        BackgroundColor = c.String(),
                    })
                .PrimaryKey(t => t.MsAppId);
            
            CreateTable(
                "dbo.WindowsAppStoreScreenshots",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 128),
                        StoreAppInfoMsAppId = c.String(),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.Link);
            
            //DropTable("dbo.AppWebLinks");
            //DropTable("dbo.StoreAppInfoes");
            //DropTable("dbo.StoreAppScreenshots");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StoreAppScreenshots",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 128),
                        StoreAppInfoMsAppId = c.String(),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.Link);
            
            CreateTable(
                "dbo.StoreAppInfoes",
                c => new
                    {
                        MsAppId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Rating = c.Double(nullable: false),
                        NumberOfRatings = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Category = c.String(),
                        AgeRating = c.String(),
                        Developer = c.String(),
                        Copyright = c.String(),
                        LogoLink = c.String(),
                        DescriptionHtml = c.String(),
                        FeaturesHtml = c.String(),
                        WebsiteLink = c.String(),
                        SupportLink = c.String(),
                        PrivacyPolicyLink = c.String(),
                        ReleaseNotes = c.String(),
                        Architecture = c.String(),
                        Languages = c.String(),
                        PackageFamilyName = c.String(),
                        AppStoreLink = c.String(),
                        BackgroundColor = c.String(),
                    })
                .PrimaryKey(t => t.MsAppId);
            
            CreateTable(
                "dbo.AppWebLinks",
                c => new
                    {
                        Link = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Link);
            
            DropTable("dbo.WindowsAppStoreScreenshots");
            DropTable("dbo.WindowsAppStoreInfoes");
            DropTable("dbo.WindowsAppStoreLinks");
        }
    }
}
