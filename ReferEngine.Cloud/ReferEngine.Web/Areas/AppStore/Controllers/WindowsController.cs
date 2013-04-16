using System;
using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using ReferEngine.Web.Controllers;

namespace ReferEngine.Web.Areas.AppStore.Controllers
{
    public class WindowsController : BaseController
    {
        public ActionResult ParentCategory(string category, string parentCategory, string name,
                            int numberOfApps = 18, int page = 1)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string appName = Util.ConvertUrlPartToString(name);
                WindowsAppViewModel windowsAppViewModel = DataOperations.GetWindowsAppViewModelByName(appName);
                if (windowsAppViewModel != null)
                {
                    windowsAppViewModel.UserAgentProperties = new UserAgentProperties(Request.UserAgent);
                    return View("WindowsApp", windowsAppViewModel);
                }
            }

            int actualNumberOfApps = numberOfApps > 50 ? 50 : numberOfApps;
            string categoryName = Util.ConvertUrlPartToString(category);
            string parentCategoryName = Util.ConvertUrlPartToString(parentCategory);
            var windowsAppStoreCategory = DataOperations.GetWindowsAppStoreCategory(categoryName, parentCategoryName);
            if (windowsAppStoreCategory != null)
            {
                var windowsCategoryViewModel = DataOperations.GetWindowsCategoryViewModel(windowsAppStoreCategory,
                                                                                          actualNumberOfApps, page);
                return View("WindowsCategory", windowsCategoryViewModel);
            }

            throw new InvalidOperationException(string.Format("Category: {0}, ParentCategory: {1}, AppName: {2}", category, parentCategory, name));
        }

        public ActionResult Category(string category = null, string name = null, int numberOfApps = 18, int page = 1)
        {
            int actualNumberOfApps = numberOfApps > 50 ? 50 : numberOfApps;
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            
            if (string.IsNullOrEmpty(category))
            {
                IList<WindowsAppStoreCategory> categories = DataOperations.GetWindowsAppStoreCategories();
                return View("WindowsStore", categories);
            }

            string categoryName = Util.ConvertUrlPartToString(category);
            var windowsAppStoreCategory = DataOperations.GetWindowsAppStoreCategory(categoryName, null);
            if (windowsAppStoreCategory == null)
            {
                IList<WindowsAppStoreCategory> categories = DataOperations.GetWindowsAppStoreCategories();
                return View("WindowsStore", categories);
            }

            WindowsAppViewModel windowsAppViewModel = null;
            if (!string.IsNullOrEmpty(name))
            {
                string appName = Util.ConvertUrlPartToString(name);
                windowsAppViewModel = DataOperations.GetWindowsAppViewModelByName(appName);
            }
            if (windowsAppViewModel == null)
            {
                var windowsCategoryViewModel = DataOperations.GetWindowsCategoryViewModel(windowsAppStoreCategory, 
                                                                actualNumberOfApps, page);
                return View("WindowsCategory", windowsCategoryViewModel);
            }

            windowsAppViewModel.UserAgentProperties = userAgentProperties;
            return View("WindowsApp", windowsAppViewModel);
        }

        // appstore/windows/a/getapps
        [HttpPost]
        public ActionResult GetApps(string searchTerm = null, string category = null, string parentCategory = null, int page = 1, int numberOfApps = 20)
        {
            int actualNumberOfApps = numberOfApps < 1 || numberOfApps > 100 ? 20 : numberOfApps;
            var apps = DataOperations.GetWindowsAppStoreInfos(searchTerm, category, parentCategory, page, actualNumberOfApps);
            List<WindowsAppStoreInfoClientSide> result = apps.Select(a => new WindowsAppStoreInfoClientSide(a)).ToList();
            return Json(result);
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
