using System.Data.Entity.Migrations;

namespace AppSmarts.Common.iOSMigrations
{
    public partial class LongDownloadSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.iOSApps", "DownloadSize", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.iOSApps", "DownloadSize", c => c.Int(nullable: false));
        }
    }
}
