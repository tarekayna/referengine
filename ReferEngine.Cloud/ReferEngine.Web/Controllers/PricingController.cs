using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class PricingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
