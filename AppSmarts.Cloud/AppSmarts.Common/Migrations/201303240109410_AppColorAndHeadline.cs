using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppColorAndHeadline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "Headline", c => c.String());
            AddColumn("dbo.Apps", "BackgroundColor", c => c.String());
            AlterColumn("dbo.Apps", "BackgroundImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "BackgroundColor");
            DropColumn("dbo.Apps", "Headline");
        }
    }
}
