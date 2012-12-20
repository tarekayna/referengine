using System.Web.Mvc;
using ReferEngine.Common.Models;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
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
