using System.Data.Entity.Migrations;

namespace AppSmarts.Common.iOSMigrations
{
    public partial class FirstMigratin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.iOSApps",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ExportDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        RecommendedAge = c.String(),
                        ArtistName = c.String(),
                        SellerName = c.String(),
                        CompanyUrl = c.String(),
                        SupportUrl = c.String(),
                        ViewUrl = c.String(),
                        ArtworkUrlLarge = c.String(),
                        ArtworkUrlSmall = c.String(),
                        iTunesReleaseDate = c.DateTime(nullable: false),
                        Copyright = c.String(),
                        Description = c.String(),
                        Version = c.String(),
                        iTunesVersion = c.String(),
                        DownloadSize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.iOSAppArtists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        Artist_Id = c.Long(),
                        App_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSArtists", t => t.Artist_Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .Index(t => t.Artist_Id)
                .Index(t => t.App_Id);
            
            CreateTable(
                "dbo.iOSArtists",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ExportDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        SearchTerms = c.String(),
                        IsActualArtist = c.String(),
                        ViewUrl = c.String(),
                        ArtistType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSArtistTypes", t => t.ArtistType_Id)
                .Index(t => t.ArtistType_Id);
            
            CreateTable(
                "dbo.iOSArtistTypes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ExportDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        MediaType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSMediaTypes", t => t.MediaType_Id)
                .Index(t => t.MediaType_Id);
            
            CreateTable(
                "dbo.iOSMediaTypes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        ExportDate = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.iOSAppDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        LanguageCode = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        ReleaseNotes = c.String(),
                        CompanyUrl = c.String(),
                        SupportUrl = c.String(),
                        App_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .Index(t => t.App_Id);
            
            CreateTable(
                "dbo.AppScreenshots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CloudinaryImage_Id = c.String(maxLength: 128),
                        iOSAppDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudinaryImages", t => t.CloudinaryImage_Id)
                .ForeignKey("dbo.iOSAppDetails", t => t.iOSAppDetail_Id)
                .Index(t => t.CloudinaryImage_Id)
                .Index(t => t.iOSAppDetail_Id);
            
            CreateTable(
                "dbo.CloudinaryImages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Format = c.String(),
                        Description = c.String(),
                        OriginalLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.iOSAppDeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        App_Id = c.Long(),
                        DeviceType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .ForeignKey("dbo.iOSDeviceTypes", t => t.DeviceType_Id)
                .Index(t => t.App_Id)
                .Index(t => t.DeviceType_Id);
            
            CreateTable(
                "dbo.iOSDeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        ExportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.iOSAppGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        IsPrimary = c.Boolean(nullable: false),
                        Genre_Id = c.Int(),
                        App_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSGenres", t => t.Genre_Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .Index(t => t.Genre_Id)
                .Index(t => t.App_Id);
            
            CreateTable(
                "dbo.iOSGenres",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ExportDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        ParentGenre_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSGenres", t => t.ParentGenre_Id)
                .Index(t => t.ParentGenre_Id);
            
            CreateTable(
                "dbo.iOSAppPopularityPerGenres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        Rank = c.Int(nullable: false),
                        Storefront_Id = c.Int(),
                        Genre_Id = c.Int(),
                        App_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSStorefronts", t => t.Storefront_Id)
                .ForeignKey("dbo.iOSGenres", t => t.Genre_Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .Index(t => t.Storefront_Id)
                .Index(t => t.Genre_Id)
                .Index(t => t.App_Id);
            
            CreateTable(
                "dbo.iOSStorefronts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        CountryCode = c.String(),
                        ExportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.iOSAppPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportDate = c.DateTime(nullable: false),
                        RetailPrice = c.String(),
                        CurrencyCode = c.String(),
                        App_Id = c.Long(),
                        Storefront_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iOSApps", t => t.App_Id)
                .ForeignKey("dbo.iOSStorefronts", t => t.Storefront_Id)
                .Index(t => t.App_Id)
                .Index(t => t.Storefront_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.iOSAppPrices", "Storefront_Id", "dbo.iOSStorefronts");
            DropForeignKey("dbo.iOSAppPrices", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.iOSAppPopularityPerGenres", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.iOSAppPopularityPerGenres", "Genre_Id", "dbo.iOSGenres");
            DropForeignKey("dbo.iOSAppPopularityPerGenres", "Storefront_Id", "dbo.iOSStorefronts");
            DropForeignKey("dbo.iOSAppGenres", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.iOSAppGenres", "Genre_Id", "dbo.iOSGenres");
            DropForeignKey("dbo.iOSGenres", "ParentGenre_Id", "dbo.iOSGenres");
            DropForeignKey("dbo.iOSAppDeviceTypes", "DeviceType_Id", "dbo.iOSDeviceTypes");
            DropForeignKey("dbo.iOSAppDeviceTypes", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.AppScreenshots", "iOSAppDetail_Id", "dbo.iOSAppDetails");
            DropForeignKey("dbo.AppScreenshots", "CloudinaryImage_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.iOSAppDetails", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.iOSAppArtists", "App_Id", "dbo.iOSApps");
            DropForeignKey("dbo.iOSAppArtists", "Artist_Id", "dbo.iOSArtists");
            DropForeignKey("dbo.iOSArtists", "ArtistType_Id", "dbo.iOSArtistTypes");
            DropForeignKey("dbo.iOSArtistTypes", "MediaType_Id", "dbo.iOSMediaTypes");
            DropIndex("dbo.iOSAppPrices", new[] { "Storefront_Id" });
            DropIndex("dbo.iOSAppPrices", new[] { "App_Id" });
            DropIndex("dbo.iOSAppPopularityPerGenres", new[] { "App_Id" });
            DropIndex("dbo.iOSAppPopularityPerGenres", new[] { "Genre_Id" });
            DropIndex("dbo.iOSAppPopularityPerGenres", new[] { "Storefront_Id" });
            DropIndex("dbo.iOSAppGenres", new[] { "App_Id" });
            DropIndex("dbo.iOSAppGenres", new[] { "Genre_Id" });
            DropIndex("dbo.iOSGenres", new[] { "ParentGenre_Id" });
            DropIndex("dbo.iOSAppDeviceTypes", new[] { "DeviceType_Id" });
            DropIndex("dbo.iOSAppDeviceTypes", new[] { "App_Id" });
            DropIndex("dbo.AppScreenshots", new[] { "iOSAppDetail_Id" });
            DropIndex("dbo.AppScreenshots", new[] { "CloudinaryImage_Id" });
            DropIndex("dbo.iOSAppDetails", new[] { "App_Id" });
            DropIndex("dbo.iOSAppArtists", new[] { "App_Id" });
            DropIndex("dbo.iOSAppArtists", new[] { "Artist_Id" });
            DropIndex("dbo.iOSArtists", new[] { "ArtistType_Id" });
            DropIndex("dbo.iOSArtistTypes", new[] { "MediaType_Id" });
            DropTable("dbo.iOSAppPrices");
            DropTable("dbo.iOSStorefronts");
            DropTable("dbo.iOSAppPopularityPerGenres");
            DropTable("dbo.iOSGenres");
            DropTable("dbo.iOSAppGenres");
            DropTable("dbo.iOSDeviceTypes");
            DropTable("dbo.iOSAppDeviceTypes");
            DropTable("dbo.CloudinaryImages");
            DropTable("dbo.AppScreenshots");
            DropTable("dbo.iOSAppDetails");
            DropTable("dbo.iOSMediaTypes");
            DropTable("dbo.iOSArtistTypes");
            DropTable("dbo.iOSArtists");
            DropTable("dbo.iOSAppArtists");
            DropTable("dbo.iOSApps");
        }
    }
}
