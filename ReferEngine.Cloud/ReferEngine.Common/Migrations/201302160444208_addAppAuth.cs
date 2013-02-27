namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAppAuth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppAuthorizations",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 128),
                        UserHostAddress = c.String(),
                        IsVerified = c.Boolean(nullable: false),
                        App_Id = c.Long(),
                        AppReceipt_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Token)
                .ForeignKey("dbo.Apps", t => t.App_Id)
                .ForeignKey("dbo.AppReceipts", t => t.AppReceipt_Id)
                .Index(t => t.App_Id)
                .Index(t => t.AppReceipt_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppAuthorizations", "AppReceipt_Id", "dbo.AppReceipts");
            DropForeignKey("dbo.AppAuthorizations", "App_Id", "dbo.Apps");
            DropIndex("dbo.AppAuthorizations", new[] { "AppReceipt_Id" });
            DropIndex("dbo.AppAuthorizations", new[] { "App_Id" });
            DropTable("dbo.AppAuthorizations");
        }
    }
}
