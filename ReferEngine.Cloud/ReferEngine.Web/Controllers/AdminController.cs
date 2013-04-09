﻿using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using ReferEngine.Common.Tracing;
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

        public ActionResult WindowsAppInvites()
        {
            return View();
        }

        public ActionResult TraceLogs(string role = "ReferEngine.WorkerCloud.WinApps", int page = 1, int pageSize = 50)
        {
            TraceLogsViewModel viewModel = new TraceLogsViewModel();
            viewModel.Role = role;
            viewModel.CurrentPage = page;
            viewModel.TraceMessages = Tracer.GetTraceMessages(role, page, pageSize);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SendWindowsAppInvite(string msAppId, string email, string name)
        {
            WindowsAppStoreInfo appStoreInfo = DataOperations.GetWindowsAppStoreInfo(msAppId);
            Invite invite = Invite.Generate(email, isRequested: false, msAppId: msAppId);
            Emailer.SendWindowsAppInvite(email, appStoreInfo, invite, name);
            DataOperations.AddInvite(invite);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
            Emailer.SendInviteEmail(invite);
            DataOperations.AddInvite(invite);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
