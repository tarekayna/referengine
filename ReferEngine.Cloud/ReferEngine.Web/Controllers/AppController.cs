using System.Linq;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReferEngine.Common.Models;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
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
            App app = DataReader.GetApp(id);
            
            if (app != null &&
               (app.UserId == WebSecurity.CurrentUserId || Roles.IsUserInRole("Admin")))
            {
                var viewModel = DataReader.GetAppDashboardViewModel(app);

                ViewBag.CurrentApp = app;
                return View(viewModel);
            }

            return RedirectToAction("Index");
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
