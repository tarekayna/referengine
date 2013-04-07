namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CloudinaryImageFormat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CloudinaryImages", "Format", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CloudinaryImages", "Format");
        }
    }
}
