using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) {}

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Use()
        {
            return View();
        }

        public ActionResult Copyright()
        {
            return View();
        }

        public ActionResult Rules(long id)
        {
            App app = DataReader.GetApp(id);
            return View(app);
        }
    }
}
