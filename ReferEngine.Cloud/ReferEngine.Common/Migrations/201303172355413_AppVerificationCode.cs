namespace ReferEngine.Common.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AppVerificationCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apps", "VerificationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apps", "VerificationCode");
        }
    }
}
