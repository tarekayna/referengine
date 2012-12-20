using System.Web.Mvc;
using ReferEngine.Web.Models.Error;

namespace ReferEngine.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string message)
        {
            return View(new ErrorViewModel(message));
        }
    }
}
