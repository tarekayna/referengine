using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class ParentCategoryName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WindowsAppStoreCategories", "ParentCategoryName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WindowsAppStoreCategories", "ParentCategoryName");
        }
    }
}
