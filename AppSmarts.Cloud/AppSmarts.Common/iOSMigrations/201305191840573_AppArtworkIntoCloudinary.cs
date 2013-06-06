using System.Data.Entity.Migrations;

namespace AppSmarts.Common.iOSMigrations
{
    public partial class AppArtworkIntoCloudinary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.iOSApps", "ArtworkLarge_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.iOSApps", "ArtworkSmall_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.iOSApps", "ArtworkLarge_Id");
            CreateIndex("dbo.iOSApps", "ArtworkSmall_Id");
            AddForeignKey("dbo.iOSApps", "ArtworkLarge_Id", "dbo.CloudinaryImages", "Id");
            AddForeignKey("dbo.iOSApps", "ArtworkSmall_Id", "dbo.CloudinaryImages", "Id");
            DropColumn("dbo.iOSApps", "ArtworkUrlLarge");
            DropColumn("dbo.iOSApps", "ArtworkUrlSmall");
        }
        
        public override void Down()
        {
            AddColumn("dbo.iOSApps", "ArtworkUrlSmall", c => c.String());
            AddColumn("dbo.iOSApps", "ArtworkUrlLarge", c => c.String());
            DropForeignKey("dbo.iOSApps", "ArtworkSmall_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.iOSApps", "ArtworkLarge_Id", "dbo.CloudinaryImages");
            DropIndex("dbo.iOSApps", new[] { "ArtworkSmall_Id" });
            DropIndex("dbo.iOSApps", new[] { "ArtworkLarge_Id" });
            DropColumn("dbo.iOSApps", "ArtworkSmall_Id");
            DropColumn("dbo.iOSApps", "ArtworkLarge_Id");
        }
    }
}
