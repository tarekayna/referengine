using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class DeleteDownLinks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WindowsAppStoreLinks", "NumberOfConsecutiveFailures", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WindowsAppStoreLinks", "NumberOfConsecutiveFailures");
        }
    }
}
