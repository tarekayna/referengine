using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppLogoLinks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "LogoLink50", c => c.String(nullable: false));
            AddColumn("dbo.Apps", "LogoLink150", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "LogoLink150");
            DropColumn("dbo.Apps", "LogoLink50");
        }
    }
}
