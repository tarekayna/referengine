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
                                Description = "Whether you're a student, a teacher or a mathematical genius in the making, Blu Graphing Calculator gives you most of the advanced functionality of a scientific calculator, with all the latest interactivity Windows 8 has to offer.",
                                AppStoreLink =
                                    "http://apps.microsoft.com/webpdp/en-US/app/blu-graphing-calculator/764cce31-8f93-48a6-b4fc-008eb78e50d4",
                                Copyright = "Copyright © 2012, Apexa Inc.",
                                Publisher = "Apexa Inc.",
                                PackageFamilyName = "Apexa.co.Calculi_pn2wyk5mfanv6",
                                ImageLink =
                                    "http://wscont1.apps.microsoft.com/winstore/1x/2460c09e-39d8-4e6f-a35a-e20c0cebaa79/Icon.20879.png",
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
