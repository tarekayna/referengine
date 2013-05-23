using System;
using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Models;
using ReferEngine.Common.Models.iOS;
using ReferEngine.Common.Utilities;
using ReferEngine.Common.ViewModels.AppStore.Windows;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using ReferEngine.Common.ViewModels.AppStore.iOS;
using ReferEngine.Web.Areas.AppStore.ViewModels;
using ReferEngine.Web.Controllers;

namespace ReferEngine.Web.Areas.AppStore.Controllers
{
    public class iPhoneAndiPadController : BaseController
    {
        //public ActionResult ParentCategory(string category, string parentCategory, string name,
        //                    int numberOfApps = 18, int page = 1)
        //{
        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        string appName = Util.ConvertUrlPartToString(name);
        //        iOSAppViewModel iOSAppViewModel = iOSDataReader.GetAppViewModelByName(appName);
        //        if (iOSAppViewModel != null)
        //        {
        //            iOSAppViewModel.UserAgentProperties = new UserAgentProperties(Request.UserAgent);
        //            return View("iOSApp", iOSAppViewModel);
        //        }
        //    }

        //    int actualNumberOfApps = numberOfApps > 50 ? 50 : numberOfApps;
        //    string categoryName = Util.ConvertUrlPartToString(category);
        //    string parentCategoryName = Util.ConvertUrlPartToString(parentCategory);
        //    var windowsAppStoreCategory = DataOperations.GetWindowsAppStoreCategory(categoryName, parentCategoryName);
        //    if (windowsAppStoreCategory != null)
        //    {
        //        var windowsCategoryViewModel = DataOperations.GetWindowsCategoryViewModel(windowsAppStoreCategory,
        //                                                                                  actualNumberOfApps, page);
        //        return View("WindowsCategory", windowsCategoryViewModel);
        //    }

        //    throw new InvalidOperationException(string.Format("Category: {0}, ParentCategory: {1}, AppName: {2}", category, parentCategory, name));
        //}

        public ActionResult Category(string category = null, string name = null, int numberOfApps = 18, int page = 1)
        {
            int actualNumberOfApps = numberOfApps > 50 ? 18 : numberOfApps;
            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);
            
            if (string.IsNullOrEmpty(category))
            {
                IList<iOSGenre> genres = iOSDataReader.GetAppStoreGenres();
                return View("iOSStore", genres);
            }

            string categoryName = Util.ConvertUrlPartToString(category);
            var windowsAppStoreCategory = DataOperations.GetWindowsAppStoreCategory(categoryName, null);
            if (windowsAppStoreCategory == null)
            {
                IList<WindowsAppStoreCategory> categories = DataOperations.GetWindowsAppStoreCategories();
                return View("WindowsStore", categories);
            }

            iOSAppViewModel appViewModel = null;
            if (!string.IsNullOrEmpty(name))
            {
                string appName = Util.ConvertUrlPartToString(name);
                appViewModel = iOSDataReader.GetAppViewModelByName(appName);
            }
            if (appViewModel == null)
            {
                var windowsCategoryViewModel = DataOperations.GetWindowsCategoryViewModel(windowsAppStoreCategory, 
                                                                actualNumberOfApps, page);
                return View("WindowsCategory", windowsCategoryViewModel);
            }

            appViewModel.UserAgentProperties = userAgentProperties;
            return View("iOSApp", appViewModel);
        }
    }
}
