using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class logoLinkHighQuality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "LogoLinkHighQuality", c => c.String());
            DropColumn("dbo.Apps", "LogoLink150");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Apps", "LogoLink150", c => c.String());
            DropColumn("dbo.Apps", "LogoLinkHighQuality");
        }
    }
}
