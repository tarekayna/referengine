namespace ReferEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Apps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppStoreLink = c.String(),
                        Name = c.String(),
                        ImageLink = c.String(),
                        Description = c.String(),
                        Platform = c.Int(nullable: false),
                        Developer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Developers", t => t.Developer_Id)
                .Index(t => t.Developer_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Apps", new[] { "Developer_Id" });
            DropForeignKey("dbo.Apps", "Developer_Id", "dbo.Developers");
            DropTable("dbo.Apps");
            DropTable("dbo.Developers");
        }
    }
}
