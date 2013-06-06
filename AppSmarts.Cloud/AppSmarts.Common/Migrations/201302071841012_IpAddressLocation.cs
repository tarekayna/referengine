using System.Data.Entity.Migrations;

namespace AppSmarts.Common.Migrations
{
    public partial class IpAddressLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IpAddressLocations",
                c => new
                    {
                        IpAddress = c.String(nullable: false, maxLength: 128),
                        Confidence = c.Short(nullable: false),
                        Results = c.String(),
                        Domain = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        ZipCode = c.String(),
                        Region = c.String(),
                        ISP = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        CountryAbbreviation = c.String(),
                    })
                .PrimaryKey(t => t.IpAddress);
            
            AddColumn("dbo.AppRecommendations", "IpAddress", c => c.String());
            AddColumn("dbo.AppReceipts", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppReceipts", "IpAddress");
            DropColumn("dbo.AppRecommendations", "IpAddress");
            DropTable("dbo.IpAddressLocations");
        }
    }
}
