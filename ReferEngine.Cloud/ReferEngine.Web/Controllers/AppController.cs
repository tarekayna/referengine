using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.Mvc;
using Itenso.TimePeriod;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Common;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    [Authorize(Roles="Admin, Dev")]
    public class AppController : BaseController
    {
        public AppController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Dashboard(long id)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            
            if (ViewProperties.CurrentApp != null &&
               (ViewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || Roles.IsUserInRole("Admin")))
            {
                var viewModel = DataReader.GetAppDashboardViewModel(ViewProperties.CurrentApp);
                return View(viewModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        public ActionResult GetAppDashboardMapData(long id, string who, string when)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = GetTimeRange(when);

            IList<IpAddressLocation> locations;
            switch (who)
            {
                case "launched":
                    locations = DatabaseOperations.GetAppLaunchLocations(ViewProperties.CurrentApp, timeRange);
                    break;
                case "intro":
                    locations = DatabaseOperations.GetAppIntroLocations(ViewProperties.CurrentApp, timeRange);
                    break;
                case "recommended":
                    locations = DatabaseOperations.GetAppRecommendLocations(ViewProperties.CurrentApp, timeRange);
                    break;
                default:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(locations.Select(l => new { l.Longitude, l.Latitude, l.City }));
        }

        //[HttpPost]
        //public ActionResult GetAppDashboardChartData(long id, string when)
        //{
            
        //}

        private TimeRange GetTimeRange(string when)
        {
            TimeRange timeRange = new TimeRange {End = DateTime.UtcNow};

            switch (when)
            {
                case "past-24-hours":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromHours(24));
                    break;
                case "past-48-hours":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromHours(48));
                    break;
                case "past-3-days":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(3));
                    break;
                case "past-7-days":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(7));
                    break;
                case "past-30-days":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(30));
                    break;
                case "past-60-days":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(60));
                    break;
                case "past-90-days":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(90));
                    break;
                case "past-year":
                    timeRange.Start = timeRange.End.Subtract(TimeSpan.FromDays(365));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return timeRange;
        }

        [HttpPost]
        public JsonResult FindStoreApps(string term)
        {
            var infos = DataReader.FindStoreApps(term, 10);
            var data = from i in infos
                       select i.Name;
            return Json(data);
        }

        //public ActionResult Edit(long id)
        //{
        //    App app = DataReader.GetApp(id);
        //    return View(app);
        //}

        //[HttpPost]
        //public ActionResult Edit(long id, FormCollection collection)
        //{
        //    App app = DataReader.GetApp(id);
        //    return View(app);
        //}

        //[HttpPost]
        //public ActionResult UploadScreenshots(int id, string description)
        //{
        //    var screenshots = new List<AppScreenshot>();
        //    foreach (string file in Request.Files)
        //    {
        //        HttpPostedFileBase fileBase = Request.Files[file];
        //        if (fileBase != null)
        //        {
        //            var screenshot = AppScreenshot.Create(fileBase, id);
        //            //var addedScreenshot = DataWriter.AddAppScreenshot(screenshot);
        //            DataWriter.AddAppScreenshot(screenshot);

        //            var addedScreenshot = DataReader.GetAppScreenshot(id, description);
        //            fileBase.InputStream.Position = 0;
        //            BlobStorageOperations.UploadAppScreenshot(addedScreenshot, fileBase.InputStream);
        //            screenshots.Add(addedScreenshot);
        //        }
        //    }

        //    return View(screenshots);
        //}
    }
}
