namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
