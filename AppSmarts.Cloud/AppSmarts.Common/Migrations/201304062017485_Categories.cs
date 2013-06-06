using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class Categories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WindowsAppStoreCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WindowsAppStoreInfoes", "Category_Id", c => c.Int());
            CreateIndex("dbo.WindowsAppStoreInfoes", "Category_Id");
            AddForeignKey("dbo.WindowsAppStoreInfoes", "Category_Id", "dbo.WindowsAppStoreCategories", "Id");
            DropColumn("dbo.WindowsAppStoreInfoes", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WindowsAppStoreInfoes", "Category", c => c.String());
            DropForeignKey("dbo.WindowsAppStoreInfoes", "Category_Id", "dbo.WindowsAppStoreCategories");
            DropIndex("dbo.WindowsAppStoreInfoes", new[] { "Category_Id" });
            DropColumn("dbo.WindowsAppStoreInfoes", "Category_Id");
            DropTable("dbo.WindowsAppStoreCategories");
        }
    }
}
