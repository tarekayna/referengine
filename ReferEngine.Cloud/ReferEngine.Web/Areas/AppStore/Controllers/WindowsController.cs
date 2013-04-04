using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Common.ViewModels.AppStore.Windows;

namespace ReferEngine.Web.Areas.AppStore.Controllers
{
    public class WindowsController : Controller
    {
        public ActionResult Index(string platform, string category = null, string name = null)
        {
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);

            if (category == null)
            {
                return View("WindowsStore");
            }

            if (name == null)
            {
                return View("WindowsCategory");
            }

            string appName = name.Replace('-', ' ');
            WindowsAppViewModel windowsAppViewModel = DataOperations.GetWindowsAppViewModelByName(appName);
            if (windowsAppViewModel != null)
            {
                windowsAppViewModel.UserAgentProperties = userAgentProperties;
                return View("WindowsApp", windowsAppViewModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public ActionResult GetAppRecommendations(long appId, int count, int start)
        {
            App app = DataOperations.GetApp(appId);
            if (app == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var data = DataOperations.GetAppRecommendationsPeople(app, count, start);
            return Json(data);
        }
    }
}
