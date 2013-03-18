using Itenso.TimePeriod;
using NodaTime;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
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

        [HttpPost]
        public ActionResult AddNewApp(string msAppId)
        {
            DatabaseOperations.AddNewAppFromStoreInfo(msAppId, ViewProperties.CurrentUser);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult SearchForApp(string name, string platform)
        {
            Verifier.IsNotNullOrEmpty(name, "name");
            Verifier.IsNotNullOrEmpty(platform, "platform");

            var infos = DataReader.FindStoreApps(name, 10);

            return Json(infos);
        }

        public ActionResult Dashboard(long id)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);

            if (ViewProperties.CurrentApp != null &&
               (ViewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || Roles.IsUserInRole("Admin")))
            {
                if (Util.CurrentServiceConfiguration != Util.ReferEngineServiceConfiguration.Local)
                {
                    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                    DatabaseOperations.GetIpAddressLocation(ip);
                }
                return View(ViewProperties.CurrentApp);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult Settings(long id)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);

            if (ViewProperties.CurrentApp != null &&
               (ViewProperties.CurrentApp.UserId == WebSecurity.CurrentUserId || Roles.IsUserInRole("Admin")))
            {
                if (Util.CurrentServiceConfiguration != Util.ReferEngineServiceConfiguration.Local)
                {
                    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                }
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        public ActionResult GetAppDashboardMapData(long id, string who, string timeZoneOffset, 
                                                   string startDate, string endDate)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            App app = ViewProperties.CurrentApp;

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);

            IList<MapUnitResult> result = DatabaseOperations.GetAppActionLocations(app, timeRange, who);
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetAppDashboardChartData(long id, string who, string timeZoneOffset,
                                                     string startDate, string endDate, string timespan)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            App app = ViewProperties.CurrentApp;

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);
            TimeSpan timeSpan = GetTimeSpan(timespan);

            var r = DatabaseOperations.GetAppActionCount(app, timeRange, timeSpan, who);
            return Json(r);
        }

        [HttpPost]
        public ActionResult GetAppDashboardPeopleData(long id, string who, string timeZoneOffset,
                                                     string startDate, string endDate)
        {
            ViewProperties.CurrentApp = DataReader.GetApp(id);
            App app = ViewProperties.CurrentApp;

            if (ViewProperties.CurrentApp == null ||
               (ViewProperties.CurrentApp.UserId != WebSecurity.CurrentUserId && !Roles.IsUserInRole("Admin")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            TimeRange timeRange = ConvertClientDateTimeToUtcTimeRange(startDate, endDate, timeZoneOffset);
            
            var r = DatabaseOperations.GetAppRecommendationsPeople(app, timeRange, who);
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
