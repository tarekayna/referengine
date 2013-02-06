namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRecommendationPageView : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecommendationPageViews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AppReceiptId = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        RecommendationPage = c.Int(nullable: false),
                        AppId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.RecommendationPageViews");
        }
    }
}
