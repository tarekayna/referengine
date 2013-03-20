namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
