using System;
using System.Web.Mvc;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class HomeController : BaseController
    {
        public HomeController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        [HttpPost]
        public ActionResult Index(string email)
        {
            if (email == null)
            {
                throw new InvalidOperationException();
            }

            PrivateBetaSignup privateBetaSignup = new PrivateBetaSignup(email);
            DataWriter.AddPrivateBetaSignup(privateBetaSignup);
            return Json(new { Email = email });           
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
