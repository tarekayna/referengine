using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppSmarts.Common.Data;
using AppSmarts.Common.Email;
using AppSmarts.Common.Models;
using AppSmarts.Common.Tracing;
using AppSmarts.Web.Models.Admin;

namespace AppSmarts.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : BaseController
    {
        public ActionResult Panel()
        {
            return View();
        }

        public ActionResult Admin()
        {
            var viewModel = new AdminViewModel();
            return View(viewModel);
        }

        public ActionResult WindowsAppInvites()
        {
            return View();
        }

        public ActionResult TraceLogs(string role = "appSmarts.iOSManager", int page = 1, int pageSize = 50)
        {
            TraceLogsViewModel viewModel = new TraceLogsViewModel
                {
                    Role = role,
                    CurrentPage = page,
                    TraceMessages = Tracer.GetTraceMessages(role, page, pageSize)
                };
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

        public ActionResult WindowsStoreCategories()
        {
            var categories = DataOperations.GetWindowsAppStoreCategories();
            return View(categories);
        }

        [HttpPost]
        public ActionResult UploadCategoryImage(string categoryId)
        {
            int windowsCategoryId = Convert.ToInt32(categoryId);
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase fileBase = Request.Files[file];
                if (fileBase != null)
                {
                    fileBase.InputStream.Position = 0;
                    CloudinaryImage image = CloudinaryConnector.UploadImage(fileBase.InputStream, fileBase.FileName);
                    DataOperations.SetWindowsCategoryImage(windowsCategoryId, image);
                    return new HttpStatusCodeResult(HttpStatusCode.OK, image.GetLink());
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}
