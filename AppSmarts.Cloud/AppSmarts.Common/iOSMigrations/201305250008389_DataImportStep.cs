using System.Data.Entity.Migrations;

namespace AppSmarts.Common.iOSMigrations
{
    public partial class DataImportStep : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.iOSDataImportSteps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateString = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ImportType = c.Int(nullable: false),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.iOSDataImports");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.iOSDataImports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsFullImport = c.Boolean(nullable: false),
                        DateString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.iOSDataImportSteps");
        }
    }
}
