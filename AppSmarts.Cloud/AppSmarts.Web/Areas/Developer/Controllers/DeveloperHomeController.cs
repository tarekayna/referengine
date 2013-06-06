using System.Web.Mvc;
using AppSmarts.Web.Controllers;

namespace AppSmarts.Web.Areas.Developer.Controllers
{
    public class DeveloperHomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }
    }
}
