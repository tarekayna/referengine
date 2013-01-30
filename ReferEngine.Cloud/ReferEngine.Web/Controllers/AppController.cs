using System.Web.Security;
using System.Web.WebPages;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.AppModels;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    [Authorize(Roles="Admin, Dev")]
    public class AppController : Controller
    {
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }

        public AppController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Dashboard(long id)
        {
            App app = DataReader.GetApp(id);
            
            if (app != null &&
               (Roles.IsUserInRole("Admin") || app.UserId == WebSecurity.CurrentUserId))
            {
                var viewModel = new AppDashboardViewModel(app);
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return View();
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
