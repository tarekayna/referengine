using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class PrivateBetaAppAndPlatform : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateBetaSignups", "AppName", c => c.String());
            AddColumn("dbo.PrivateBetaSignups", "Platforms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateBetaSignups", "Platforms");
            DropColumn("dbo.PrivateBetaSignups", "AppName");
        }
    }
}
