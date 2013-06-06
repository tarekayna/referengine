using AppSmarts.Common.Data;
using AppSmarts.Common.Models;
using AppSmarts.Common.Utilities;
using AppSmarts.Web.Models.Common;
using Itenso.TimePeriod;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace AppSmarts.Web.Controllers
{
    [Authorize(Roles="Admin, Dev")]
    public class AppController : BaseController
    {
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewApp(string msAppId)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            App app = DataOperations.AddNewAppFromStoreInfo(msAppId, viewProperties.CurrentUser);
            return Json(app);
        }

        [HttpPost]
        public ActionResult SearchForApp(string name, string platform)
        {
            Verifier.IsNotNullOrEmpty(name, "name");
            Verifier.IsNotNullOrEmpty(platform, "platform");

            var infos = DataOperations.GetWindowsAppStoreInfos(name, null, null, 1, 10);

            return Json(infos);
        }

        public ActionResult Dashboard(long id)
        {
            ViewProperties viewProperties = ((ViewProperties) ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);
            viewProperties.ActiveMenuItem = "Dashboard";

            if (viewProperties.CurrentApp != null &&
               (viewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || User.IsInRole(Roles.Admin)))
            {
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        public ActionResult GetAppDashboardMapData(long id, string who, string timeZoneOffset, 
                                                   string startDate, string endDate)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);

            if (viewProperties.CurrentApp == null ||
               (viewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !User.IsInRole(Roles.Admin)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);

            IList<MapUnitResult> result = DataOperations.GetAppActionLocations(viewProperties.CurrentApp, timeRange, who);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetAppDashboardChartData(long id, string who, string timeZoneOffset,
                                                     string startDate, string endDate, string timespan)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);
            App app = viewProperties.CurrentApp;

            if (viewProperties.CurrentApp == null ||
               (viewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !User.IsInRole(Roles.Admin)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);
            TimeSpan timeSpan = GetTimeSpan(timespan);

            var r = DataOperations.GetAppActionCount(app, timeRange, timeSpan, who);
            return Json(r);
        }

        [HttpPost]
        public ActionResult GetAppDashboardPeopleData(long id, string who, string timeZoneOffset,
                                                     string startDate, string endDate)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);
            App app = viewProperties.CurrentApp;

            if (viewProperties.CurrentApp == null ||
               (viewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !User.IsInRole(Roles.Admin)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);
            
            var r = DataOperations.GetAppRecommendationsPeople(app, timeRange, who);
            return Json(r);
        }

        public ActionResult Settings(long id, bool? first)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);
            viewProperties.ActiveMenuItem = "Settings";

            if (viewProperties.CurrentApp != null &&
               (viewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || User.IsInRole(Roles.Admin)))
            {
                return View(first);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult Delete(long id)
        {            
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.CurrentApp = DataOperations.GetApp(id);
            viewProperties.ActiveMenuItem = "Settings";

            if (AuthorizeUserForApp())
            {
                DataOperations.SetAppAsInActive(viewProperties.CurrentApp);
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        private bool AuthorizeUserForApp()
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            return viewProperties.CurrentApp != null &&
                   (viewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || User.IsInRole(Roles.Admin));
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

        private TimeRange ConvertClientDateTimeToUtcTimeRange(string start, string end, string offset)
        {
            var timeZoneOffset = offset;
            if (timeZoneOffset.EndsWith("30"))
            {
                timeZoneOffset = timeZoneOffset.Replace("30", "00");
            }
            TimeSpan offsetTimeSpan = TimeSpan.FromHours(Convert.ToDouble(timeZoneOffset) / 100);

            DateTime clientStart = Convert.ToDateTime(start);
            DateTimeOffset clientStartOffset = new DateTimeOffset(clientStart, offsetTimeSpan);
            Instant clientStartInstant = Instant.FromDateTimeOffset(clientStartOffset);

            DateTime clientEnd = Convert.ToDateTime(end);
            DateTimeOffset clientEndOffset = new DateTimeOffset(clientEnd, offsetTimeSpan);
            Instant clientEndInstant = Instant.FromDateTimeOffset(clientEndOffset);

            DateTime utcStart = clientStartInstant.ToDateTimeUtc();
            DateTime utcEnd = clientEndInstant.ToDateTimeUtc();
            return new TimeRange(utcStart, utcEnd);
        }

    }
}
