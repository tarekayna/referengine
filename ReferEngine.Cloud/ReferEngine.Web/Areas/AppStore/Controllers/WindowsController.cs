using System;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using System.Net;
using System.Web.Mvc;

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

        [HttpPost]
        public ActionResult GetAppRecommendations(long appId, string page)
        {
            int pageNumber = Convert.ToInt32(page);
            App app = DataOperations.GetApp(appId);
            if (app == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            // 1 > 1
            // 2 > 11
            // 3 > 21
            int count = 10;
            int start = 1 + count * (pageNumber - 1);
            var data = DataOperations.GetAppRecommendationsPeople(app, count, start);
            return Json(data);
        }
    }
}
