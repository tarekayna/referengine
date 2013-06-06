using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class timeStampToAuth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppAuthorizations", "TimeStamp", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppAuthorizations", "TimeStamp");
        }
    }
}
