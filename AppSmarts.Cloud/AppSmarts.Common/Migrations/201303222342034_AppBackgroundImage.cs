using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppBackgroundImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "BackgroundImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "BackgroundImage");
        }
    }
}
