namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logoLinkHighQuality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "LogoLinkHighQuality", c => c.String());
            DropColumn("dbo.Apps", "LogoLink150");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Apps", "LogoLink150", c => c.String());
            DropColumn("dbo.Apps", "LogoLinkHighQuality");
        }
    }
}
