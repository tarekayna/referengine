//using ReferEngine.Common.Data;
//using ReferEngine.Common.Models;
//using ReferEngine.Common.Utilities;
//using System;
//using System.Net;
//using System.Web.Mvc;
//using ReferEngine.Common.ViewModels.AppStore.Windows;

//namespace ReferEngine.Web.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class AppStoreController : BaseController
//    {
//        public ActionResult Index(string platform, string category = null, string name = null)
//        {
//            UserAgentProperties userAgentProperties = new UserAgentProperties(Request.UserAgent);

//            if (platform.Equals("Windows", StringComparison.InvariantCultureIgnoreCase))
//            {
//                if (category == null)
//                {
//                    return View("Windows/WindowsStore");
//                }

//                if (name == null)
//                {
//                    return View("Windows/WindowsCategory");
//                }
                
//                string appName = name.Replace('-', ' ');
//                WindowsAppStoreInfo windowsAppStoreInfo = DataOperations.GetWindowsAppStoreInfoByName(appName);
//                if (windowsAppStoreInfo != null)
//                {
//                    WindowsAppViewModel windowsAppViewModel = new WindowsAppViewModel(windowsAppStoreInfo,
//                                                                                      userAgentProperties);
//                    return View("Windows/WindowsApp", windowsAppViewModel);
//                }
//            }

//            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
//        }
//    }
//}
