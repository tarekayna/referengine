using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferEngine.Web.Areas.Developer.Controllers
{
    public class DeveloperHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }
    }
}
