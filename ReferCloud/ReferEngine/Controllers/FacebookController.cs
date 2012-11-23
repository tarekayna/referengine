using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Filters;

namespace ReferEngine.Controllers
{
    public class FacebookController : Controller
    {
        public ActionResult App(string id)
        {
            string appId = Server.HtmlEncode(id);

            return View();
        }

    }
}
