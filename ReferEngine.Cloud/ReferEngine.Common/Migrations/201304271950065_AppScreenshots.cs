namespace ReferEngine.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppScreenshots : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CloudinaryImages", "App_Id", "dbo.Apps");
            DropForeignKey("dbo.CloudinaryImages", "WindowsAppStoreInfo_MsAppId", "dbo.WindowsAppStoreInfoes");
            DropIndex("dbo.CloudinaryImages", new[] { "App_Id" });
            DropIndex("dbo.CloudinaryImages", new[] { "WindowsAppStoreInfo_MsAppId" });
            CreateTable(
                "dbo.AppScreenshots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CloudinaryImage_Id = c.String(maxLength: 128),
                        App_Id = c.Long(),
                        WindowsAppStoreInfo_MsAppId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudinaryImages", t => t.CloudinaryImage_Id)
                .ForeignKey("dbo.Apps", t => t.App_Id)
                .ForeignKey("dbo.WindowsAppStoreInfoes", t => t.WindowsAppStoreInfo_MsAppId)
                .Index(t => t.CloudinaryImage_Id)
                .Index(t => t.App_Id)
                .Index(t => t.WindowsAppStoreInfo_MsAppId);
            
            AddColumn("dbo.WindowsAppStoreCategories", "CloudinaryImage_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.WindowsAppStoreCategories", "CloudinaryImage_Id");
            AddForeignKey("dbo.WindowsAppStoreCategories", "CloudinaryImage_Id", "dbo.CloudinaryImages", "Id");

            //Sql("insert into AppScreenshots (CloudinaryImage_Id, App_Id, WindowsAppStoreInfo_MsAppId) select Id, App_Id, WindowsAppStoreInfo_MsAppId  from CloudinaryImages where WindowsAppStoreInfo_MsAppId IS NOT NULL");
            //Sql("update CloudinaryImages set App_Id = NULL, WindowsAppStoreInfo_MsAppId = NULL");

            //DropColumn("dbo.CloudinaryImages", "App_Id");
            //DropColumn("dbo.CloudinaryImages", "WindowsAppStoreInfo_MsAppId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CloudinaryImages", "WindowsAppStoreInfo_MsAppId", c => c.String(maxLength: 128));
            AddColumn("dbo.CloudinaryImages", "App_Id", c => c.Long());
            DropForeignKey("dbo.AppScreenshots", "WindowsAppStoreInfo_MsAppId", "dbo.WindowsAppStoreInfoes");
            DropForeignKey("dbo.AppScreenshots", "App_Id", "dbo.Apps");
            DropForeignKey("dbo.AppScreenshots", "CloudinaryImage_Id", "dbo.CloudinaryImages");
            DropForeignKey("dbo.WindowsAppStoreCategories", "CloudinaryImage_Id", "dbo.CloudinaryImages");
            DropIndex("dbo.AppScreenshots", new[] { "WindowsAppStoreInfo_MsAppId" });
            DropIndex("dbo.AppScreenshots", new[] { "App_Id" });
            DropIndex("dbo.AppScreenshots", new[] { "CloudinaryImage_Id" });
            DropIndex("dbo.WindowsAppStoreCategories", new[] { "CloudinaryImage_Id" });
            DropColumn("dbo.WindowsAppStoreCategories", "CloudinaryImage_Id");
            DropTable("dbo.AppScreenshots");
            CreateIndex("dbo.CloudinaryImages", "WindowsAppStoreInfo_MsAppId");
            CreateIndex("dbo.CloudinaryImages", "App_Id");
            AddForeignKey("dbo.CloudinaryImages", "WindowsAppStoreInfo_MsAppId", "dbo.WindowsAppStoreInfoes", "MsAppId");
            AddForeignKey("dbo.CloudinaryImages", "App_Id", "dbo.Apps", "Id");
        }
    }
}
