using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class InviteUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invites", "IsRequested", c => c.Boolean(nullable: true));
            AddColumn("dbo.Invites", "MsAppId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invites", "MsAppId");
            DropColumn("dbo.Invites", "IsRequested");
        }
    }
}
