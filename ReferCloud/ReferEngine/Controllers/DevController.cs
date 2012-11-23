using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReferEngine.Controllers
{
    //[Authorize(Roles = "Dev")]
    public class DevController : Controller
    {
        public ActionResult CreateApp()
        {
            return View();
        }
    }
}
