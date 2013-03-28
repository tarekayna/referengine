namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        VerificationCode = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Email);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Invites");
        }
    }
}
