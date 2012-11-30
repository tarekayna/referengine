namespace ReferLib.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ReferLib.ReferDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ReferLib.ReferDb context)
        {
            Developer developer = new Developer
                                      {
                                          FirstName = "Tarek",
                                          LastName = "Ayna",
                                          Email = "tarek@apexa.co",
                                          City = "Seattle",
                                          State = "WA",
                                          Country = "USA",
                                          PhoneNumber = "3522156216"
                                      };

            context.Developers.AddOrUpdate(developer);

            int id = context.Developers.First(d => d.Email == "tarek@apexa.co").Id;
            App app = new App
                          {
                              DeveloperId = id,
                              Name = "Blu Graphing Calculator",
                              Description = "Blu Graphing Calculator is calculator reimagined",
                              AppStoreLink = "http://apps.microsoft.com/webpdp/en-US/app/blu-graphing-calculator/764cce31-8f93-48a6-b4fc-008eb78e50d4",
                              Copyright = "Copyright © 2012, Apexa Inc.",
                              Publisher = "Apexa Inc.",
                              Platform = AppPlatform.Windows8,
                              ImageLink = "http://wscont1.apps.microsoft.com/winstore/1x/2460c09e-39d8-4e6f-a35a-e20c0cebaa79/Icon.20879.png"
                          };
            context.Apps.AddOrUpdate(app);
        }
    }
}
