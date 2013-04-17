using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using System.Net;
using System.Web.Mvc;

namespace ReferEngine.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestAnInvite(string email, string appName, string platforms)
        {
            Verifier.IsNotNullOrEmpty(email, "email");

            PrivateBetaSignup privateBetaSignup = new PrivateBetaSignup(email)
            {
                AppName = appName,
                Platforms = platforms
            };

            DataOperations.AddPrivateBetaSignup(privateBetaSignup);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Contact", "About");
        }

        public ActionResult BluGraphingCalculator()
        {
            return RedirectToAction("BluGraphingCalculator", "About");
        }

        public ActionResult BluGraphingCalculatorPrivacy()
        {
            return RedirectToAction("BluGraphingCalculatorPrivacy", "About");
        }
    }
}
