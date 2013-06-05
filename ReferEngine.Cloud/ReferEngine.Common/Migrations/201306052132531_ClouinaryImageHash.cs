namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
