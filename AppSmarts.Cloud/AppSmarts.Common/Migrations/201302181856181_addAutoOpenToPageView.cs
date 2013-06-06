using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class addAutoOpenToPageView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecommendationPageViews", "IsAutoOpen", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecommendationPageViews", "IsAutoOpen");
        }
    }
}
