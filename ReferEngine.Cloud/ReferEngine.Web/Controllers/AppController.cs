using ReferEngine.Common.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class AppController : Controller
    {
        private IReferDataReader DataReader { get; set; }
        private IReferDataWriter DataWriter { get; set; }

        public AppController(IReferDataReader dataReader, IReferDataWriter dataWriter)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
        }

        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        //public ActionResult Details(long id)
        //{
        //    App app = DataReader.GetApp(id);
        //    return View(app);
        //}

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
