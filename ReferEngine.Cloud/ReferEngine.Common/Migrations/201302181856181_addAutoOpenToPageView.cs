namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
