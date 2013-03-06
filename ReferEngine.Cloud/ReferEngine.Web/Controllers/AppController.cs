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
                return View(ViewProperties.CurrentApp);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        public ActionResult GetAppDashboardMapData(long id, string who, string startDate, string endDate)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            App app = ViewProperties.CurrentApp;

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);
            TimeRange timeRange = new TimeRange(start, end);

            IList<MapUnitResult> result = DatabaseOperations.GetAppActionLocations(app, timeRange, who);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetAppDashboardChartData(long id, string who, string startDate, string endDate, string timespan)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            App app = ViewProperties.CurrentApp;

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            DateTime start = Convert.ToDateTime(startDate);
            DateTime end = Convert.ToDateTime(endDate);
            TimeRange timeRange = new TimeRange(start, end);
            TimeSpan timeSpan = GetTimeSpan(timespan);

            var r = DatabaseOperations.GetAppActionCount(app, timeRange, timeSpan, who);
            return Json(r);
        }

        private static TimeSpan GetTimeSpan(string timespan)
        {
            switch (timespan)
            {
                case "day":
                    return TimeSpan.FromDays(1);
                case "hour":
                    return TimeSpan.FromHours(1);
                case "minute":
                    return TimeSpan.FromMinutes(1);
                default:
                    throw new InvalidOperationException();
            }
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
