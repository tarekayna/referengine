using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class authUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppAuthorizations", "ExpiresAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.AppReceipts", "IpAddress");
            DropColumn("dbo.AppAuthorizations", "IsVerified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppAuthorizations", "IsVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppReceipts", "IpAddress", c => c.String());
            DropColumn("dbo.AppAuthorizations", "ExpiresAt");
        }
    }
}
