using System;
using System.Net;
using System.Web.Mvc;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using ReferEngine.Web.Models.Common;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class HomeController : BaseController
    {
        public HomeController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [HttpGet]
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

            DataWriter.AddPrivateBetaSignup(privateBetaSignup);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
