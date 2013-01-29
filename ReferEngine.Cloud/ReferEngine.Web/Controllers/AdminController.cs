using System.Web.Mvc;
using ReferEngine.Web.Models.Admin;

namespace ReferEngine.Web.Controllers
{
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
