using System.Web.Mvc;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.Models.Admin;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        public ActionResult Admin()
        {
            var viewModel = new AdminViewModel();
            return View(viewModel);
        }

    }
}
