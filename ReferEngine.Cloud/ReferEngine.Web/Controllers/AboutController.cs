using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.Controllers
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
