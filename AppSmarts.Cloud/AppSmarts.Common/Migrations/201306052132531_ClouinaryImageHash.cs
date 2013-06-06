using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class ClouinaryImageHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CloudinaryImages", "OriginalLinkHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CloudinaryImages", "OriginalLinkHash");
        }
    }
}
