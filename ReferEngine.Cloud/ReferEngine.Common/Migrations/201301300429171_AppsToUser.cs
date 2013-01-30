namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppsToUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Apps", "UserId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Apps", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apps", "UserId", "dbo.Users");
            DropIndex("dbo.Apps", new[] { "UserId" });
            AlterColumn("dbo.Apps", "UserId", c => c.Long(nullable: false));
        }
    }
}
