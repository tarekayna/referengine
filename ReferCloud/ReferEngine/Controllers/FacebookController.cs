using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.DataAccess;
using ReferEngine.Utilities;
using ReferLib;

namespace ReferEngine.Controllers
{
    public class FacebookController : Controller
    {
        public ActionResult App(string id, string fb_action_ids, string fb_source, string action_object_map,
            string action_type_map, string action_ref_map)
        {
            int inputId;
            if (id != null && Util.TryConvertToInt(id, out inputId))
            {
                App app;
                if (DataOperations.TryGetApp(inputId, out app))
                {
                    return View(app);
                }
            }

            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
        }
    }
}
