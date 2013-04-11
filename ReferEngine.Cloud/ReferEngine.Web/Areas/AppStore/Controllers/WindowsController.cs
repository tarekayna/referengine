using System;
using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using System.Net;
using System.Web.Mvc;
using ReferEngine.Web.Controllers;

namespace ReferEngine.Web.Areas.AppStore.Controllers
{
    public class WindowsController : BaseController
    {
        public ActionResult Index(string platform, string category = null, string name = null, int numberOfApps = 20, int page = 1)
        {
            int actualNumberOfApps = numberOfApps > 50 ? 50 : numberOfApps;
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            
            if (string.IsNullOrEmpty(category))
            {
                IList<WindowsAppStoreCategory> categories = DataOperations.GetWindowsAppStoreCategories();
                return View("WindowsStore", categories);
            }
            string categoryName = category.Replace('-', ' ').Replace("and", "&");
            var windowsAppStoreCategory = DataOperations.GetWindowsAppStoreCategory(categoryName);
            if (windowsAppStoreCategory == null)
            {
                IList<WindowsAppStoreCategory> categories = DataOperations.GetWindowsAppStoreCategories();
                return View("WindowsStore", categories);
            }

            WindowsAppViewModel windowsAppViewModel = null;
            if (!string.IsNullOrEmpty(name))
            {
                string appName = name.Replace('-', ' ');
                windowsAppViewModel = DataOperations.GetWindowsAppViewModelByName(appName);
            }
            if (windowsAppViewModel == null)
            {
                WindowsCategoryViewModel windowsCategoryViewModel = DataOperations.GetWindowsCategoryViewModel(categoryName, actualNumberOfApps, page);
                return View("WindowsCategory", windowsCategoryViewModel);
            }

            windowsAppViewModel.UserAgentProperties = userAgentProperties;
            return View("WindowsApp", windowsAppViewModel);
        }

        // appstore/windows/a/getapprecommendations
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
