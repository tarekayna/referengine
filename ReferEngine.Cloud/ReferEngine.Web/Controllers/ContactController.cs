using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReferEngine.Common.Email;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(string email, string message, string subject, string name)
        {
            if (email != null && message != null)
            {
                string body = "Name = " + name + "\n";
                body += "Email = " + email + "\n";
                body += "Subject = " + subject + "\n";
                body += "Message = " + message + "\n";
                ReferEmailer.SendPlainTextEmail("tarek@referengine.com", "Contact From ReferEngine.com", body);
                return Json(new {success = true});
            }

            throw new InvalidOperationException();
        }
    }
}
