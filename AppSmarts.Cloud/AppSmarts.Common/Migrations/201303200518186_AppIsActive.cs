using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class AppIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "IsActive");
        }
    }
}
