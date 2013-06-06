using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppStoreLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreAppInfoes", "AppStoreLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreAppInfoes", "AppStoreLink");
        }
    }
}
