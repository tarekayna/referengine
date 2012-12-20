using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngineWeb.DataAccess;
using ReferEngineWeb.Utilities;
using ReferLib;

namespace ReferEngineWeb.Controllers
{
    public class FacebookController : Controller
    {
        private IReferDataReader DataReader { get; set; }

        public FacebookController(IReferDataReader dataReader)
        {
            DataReader = dataReader;
        }

        public ActionResult App(long id, string fb_action_ids, string fb_source, string action_object_map,
            string action_type_map, string action_ref_map)
        {
            App app = DataReader.GetApp(id);
            return View(app);
        }
    }
}
