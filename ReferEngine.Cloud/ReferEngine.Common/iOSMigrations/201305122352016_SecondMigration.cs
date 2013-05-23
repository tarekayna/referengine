namespace ReferEngine.Common.iOSMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.iOSArtists", "IsActualArtist", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.iOSArtists", "IsActualArtist", c => c.String());
        }
    }
}
