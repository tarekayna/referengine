using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AddIpAddressToView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecommendationPageViews", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecommendationPageViews", "IpAddress");
        }
    }
}
