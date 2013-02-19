namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appAutoShowOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppAutoShowOptions",
                c => new
                    {
                        AppId = c.Long(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Interval = c.Int(nullable: false),
                        Timeout = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppAutoShowOptions");
        }
    }
}
