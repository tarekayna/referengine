namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
