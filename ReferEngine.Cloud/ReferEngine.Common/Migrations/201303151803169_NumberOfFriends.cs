namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumberOfFriends : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "NumberOfFriends", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "NumberOfFriends");
        }
    }
}
