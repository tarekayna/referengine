using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using ReferEngine.Web.Models.Admin;

namespace ReferEngine.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : BaseController
    {
        public ActionResult Admin()
        {
            var viewModel = new AdminViewModel();
            return View(viewModel);
        }

        public ActionResult InviteRequests()
        {
            IList<PrivateBetaSignup> signups = DataOperations.GetPrivateBetaSignups();
            return View(signups);
        }

        [HttpPost]
        public ActionResult SendInvite(string email)
        {
            Invite invite = DataOperations.GetInvite(email);
            if (invite != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Invite already sent to this email.");
            }

            invite = Invite.Generate(email);
            ReferEmailer.SendInviteEmail(invite);
            DataOperations.AddInvite(invite);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
