using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppRewardPlan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppRewardPlans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        NumberOfRecommendations = c.Int(nullable: false),
                        CashAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Apps", "RewardPlan_Id", c => c.Long());
            CreateIndex("dbo.Apps", "RewardPlan_Id");
            AddForeignKey("dbo.Apps", "RewardPlan_Id", "dbo.AppRewardPlans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apps", "RewardPlan_Id", "dbo.AppRewardPlans");
            DropIndex("dbo.Apps", new[] { "RewardPlan_Id" });
            DropColumn("dbo.Apps", "RewardPlan_Id");
            DropTable("dbo.AppRewardPlans");
        }
    }
}
