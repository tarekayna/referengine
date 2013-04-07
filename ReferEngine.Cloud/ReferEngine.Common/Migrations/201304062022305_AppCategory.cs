namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "Category_Id", c => c.Int());
            CreateIndex("dbo.Apps", "Category_Id");
            AddForeignKey("dbo.Apps", "Category_Id", "dbo.WindowsAppStoreCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apps", "Category_Id", "dbo.WindowsAppStoreCategories");
            DropIndex("dbo.Apps", new[] { "Category_Id" });
            DropColumn("dbo.Apps", "Category_Id");
        }
    }
}
