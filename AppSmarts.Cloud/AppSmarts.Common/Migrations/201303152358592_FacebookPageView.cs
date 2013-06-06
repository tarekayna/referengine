using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class FacebookPageView : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacebookPageViews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        AppId = c.Long(nullable: false),
                        IpAddress = c.String(),
                        AppRecommendationFacebookPostId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FacebookPageViews");
        }
    }
}
