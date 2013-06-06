using System;
using System.Web.Mvc;
using AppSmarts.Common.Email;

namespace AppSmarts.Web.Controllers
{
    public class ContactController : BaseController
    {
        [HttpPost]
        public ActionResult SendMessage(string email, string message, string subject, string name)
        {
            if (email != null && message != null)
            {
                string body = "Name = " + name + "\n";
                body += "Email = " + email + "\n";
                body += "Subject = " + subject + "\n";
                body += "Message = " + message + "\n";
                Emailer.SendEmail("tarek@appSmarts.com", "Contact From appSmarts.com", body);
                return Json(new {success = true});
            }

            throw new InvalidOperationException();
        }
    }
}
