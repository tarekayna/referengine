using System.Web.Mvc;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Controllers
{
    [RequireHttpsPermanentRemote]
    public class VideoController : Controller
    {
        public ActionResult Team()
        {
            return View("TeamVideo");
        }

        public ActionResult Product()
        {
            return View("ProductVideo");
        }
    }
}
