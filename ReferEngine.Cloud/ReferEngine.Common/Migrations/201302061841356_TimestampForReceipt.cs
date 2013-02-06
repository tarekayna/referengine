namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimestampForReceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppReceipts", "Timestamp", c => c.DateTime(nullable: false, defaultValue: DateTime.UtcNow));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppReceipts", "Timestamp");
        }
    }
}
