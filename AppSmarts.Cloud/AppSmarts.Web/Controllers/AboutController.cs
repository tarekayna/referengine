using System.Web.Mvc;
using AppSmarts.Common.Data;
using AppSmarts.Common.Models;

namespace AppSmarts.Web.Controllers
{
    public class AboutController : BaseController
    {
        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Use()
        {
            return View();
        }

        public ActionResult Copyright()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Rules(long id)
        {
            App app = DataOperations.GetApp(id);
            return View(app);
        }

        public ActionResult BluGraphingCalculator()
        {
            return View();
        }

        public ActionResult BluGraphingCalculatorPrivacy()
        {
            return View();
        }
    }
}
