using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ReferEngineDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ReferEngineDatabaseContext context)
        {
            Developer developer = new Developer
                                      {
                                          FirstName = "Tarek",
                                          LastName = "Ayna",
                                          Email = "tarek@apexa.co",
                                          City = "Seattle",
                                          State = "WA",
                                          Country = "USA",
                                          PhoneNumber = "3522156216",
                                          ZipCode = "98102"
                                      };

            context.Developers.AddOrUpdate(d => d.Id, developer);

            context.SaveChanges();
            Int64 id = context.Developers.First(d => d.Email == "tarek@apexa.co").Id;

            App app = new App
                            {
                                DeveloperId = id,
                                Name = "Blu Graphing Calculator",
                                Description = "Ever looked at a graph and wished you could touch it? Specially engineered for Windows 8, Blu Graphing Calculator is far more than just a scientific and graphing calculator… it’s a completely interactive experience that lets you quickly create visual representations of mathematical functions. Designed to let you pinch, pull, zoom and more, Blu Graphing Calculator brings complex calculations and sophisticated graphing to a whole new level.",
                                AppStoreLink =
                                    "http://apps.microsoft.com/webpdp/en-US/app/blu-graphing-calculator/764cce31-8f93-48a6-b4fc-008eb78e50d4",
                                Copyright = "Copyright © 2012, Apexa Inc.",
                                Publisher = "Apexa Inc.",
                                PackageFamilyName = "Apexa.co.Calculi_pn2wyk5mfanv6",
                                LogoLink50 = "https://referenginestorage.blob.core.windows.net/app-logos/blu-graphing-calculator-logo-50.png",
                                LogoLink150 = "https://referenginestorage.blob.core.windows.net/app-logos/blu-graphing-calculator-logo-150.png",
                                ShortDescription = "Whether you're a student, a teacher or a mathematical genius in the making, Blu Graphing Calculator gives you most of the advanced functionality of a scientific calculator, with all the latest interactivity Windows 8 has to offer.",
                                VimeoLink = "https://player.vimeo.com/video/52197092"
                            };
            context.Apps.AddOrUpdate(a => a.Id, app);
            context.SaveChanges();
            Int64 appId = context.Apps.First(d => d.Name == "Blu Graphing Calculator").Id;

            List<ScreenshotInfo> screenshots = new List<ScreenshotInfo>
                {
                    new ScreenshotInfo("Graphing Mode", "Graphing Mode"),
                    new ScreenshotInfo("Calculator Mode", "Calculator Mode"),
                    new ScreenshotInfo("Graphing Menu", "Graphing Menu"),
                    new ScreenshotInfo("Intersection Points", "Intersection Points"),
                    new ScreenshotInfo("Scratch On", "Scratch On"),
                    new ScreenshotInfo("Table View", "Table View"),
                    new ScreenshotInfo("Snap View", "Snap View"),
                };

            for (int i = 0; i < screenshots.Count; i++)
            {
                var screenshotInfo = screenshots[i];
                AppScreenshot screenshot = new AppScreenshot
                {
                    Id = i,
                    AppId = appId,
                    Description = screenshotInfo.Description,
                    Title = screenshotInfo.Title
                };
                context.AppScreenshots.AddOrUpdate(screenshot);
            }
        }

        private class ScreenshotInfo
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public ScreenshotInfo(string title, string desc)
            {
                Title = title;
                Description = desc;
            }
        }
    }
}
