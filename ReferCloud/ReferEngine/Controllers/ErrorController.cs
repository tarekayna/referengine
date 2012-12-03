using System.Web.Mvc;
using ReferEngine.Models.Error;

namespace ReferEngine.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string message)
        {
            return View(new ErrorViewModel(message));
        }
    }
}
