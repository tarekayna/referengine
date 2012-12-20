using System.Web.Mvc;
using ReferEngineWeb.Models.Error;

namespace ReferEngineWeb.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string message)
        {
            return View(new ErrorViewModel(message));
        }
    }
}
