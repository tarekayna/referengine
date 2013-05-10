using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Web.Controllers;

namespace ReferEngine.Web.Areas.Developer.Controllers
{
    public class DeveloperHomeController : BaseController
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
