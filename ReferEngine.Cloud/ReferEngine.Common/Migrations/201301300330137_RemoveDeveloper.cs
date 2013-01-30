namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDeveloper : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Apps", "DeveloperId", "dbo.Developers");
            DropIndex("dbo.Apps", new[] { "DeveloperId" });

            AddColumn("dbo.Apps", "UserId", c => c.Int(nullable: false, defaultValue: 2));
            CreateIndex("dbo.Apps", "UserId");
            AddForeignKey("dbo.Apps", "UserId", "dbo.Users", "Id");
            
            DropColumn("dbo.Apps", "DeveloperId");
            DropTable("dbo.Developers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Developers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Apps", "DeveloperId", c => c.Long(nullable: false));
            DropForeignKey("dbo.Apps", "User_Id", "dbo.Users");
            DropIndex("dbo.Apps", new[] { "User_Id" });
            DropColumn("dbo.Apps", "User_Id");
            DropColumn("dbo.Apps", "UserId");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.Users");
            CreateIndex("dbo.Apps", "DeveloperId");
            AddForeignKey("dbo.Apps", "DeveloperId", "dbo.Developers", "Id", cascadeDelete: true);
        }
    }
}
