namespace ReferEngine.Common.iOSMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OriginalLinkHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CloudinaryImages", "OriginalLinkHash", c => c.String(true, 400));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CloudinaryImages", "OriginalLinkHash");
        }
    }
}
