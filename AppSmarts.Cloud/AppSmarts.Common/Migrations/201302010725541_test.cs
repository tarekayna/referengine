using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "Person_FacebookId", "dbo.People");
            DropForeignKey("dbo.People", "CurrentUser_FacebookId", "dbo.People");
            DropIndex("dbo.People", new[] { "Person_FacebookId" });
            DropIndex("dbo.People", new[] { "CurrentUser_FacebookId" });
            DropColumn("dbo.People", "Discriminator");
            DropColumn("dbo.People", "Person_FacebookId");
            DropColumn("dbo.People", "CurrentUser_FacebookId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "CurrentUser_FacebookId", c => c.Long());
            AddColumn("dbo.People", "Person_FacebookId", c => c.Long());
            AddColumn("dbo.People", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.People", "CurrentUser_FacebookId");
            CreateIndex("dbo.People", "Person_FacebookId");
            AddForeignKey("dbo.People", "CurrentUser_FacebookId", "dbo.People", "FacebookId");
            AddForeignKey("dbo.People", "Person_FacebookId", "dbo.People", "FacebookId");
        }
    }
}
