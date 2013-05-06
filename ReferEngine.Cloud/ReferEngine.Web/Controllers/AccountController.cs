using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using ReferEngine.Common.Data;
using ReferEngine.Common.Email;
using ReferEngine.Common.Models;
using ReferEngine.Web.Models.Common;
using ReferEngine.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ReferEngine.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string successMessage)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.SuccessMessage = successMessage;
            viewProperties.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(model.Email, model.Password, model.RememberMe))
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        var user = DataOperations.GetUser(model.Email);
                        return user.Apps.Any() ? RedirectToLocal("/app/dashboard/" + user.Apps.First().Id) : RedirectToLocal("/app/new");
                    }
                    
                    return RedirectToLocal(returnUrl);
                }

                if (WebSecurity.UserExists(model.Email))
                {
                    if (!WebSecurity.IsConfirmed(model.Email))
                    {
                        User user = DataOperations.GetUser(model.Email);
                        if (user != null)
                        {
                            ConfirmationCodeModel confirmModel = new ConfirmationCodeModel
                            {
                                Email = model.Email,
                                FirstName = user.FirstName,
                            };

                            TempData.Add("ConfirmationCodeModel", confirmModel);
                            return RedirectToAction("ConfirmYourAccount", new { SuccessMessage = "Your account has been confirmed! Please log in to start.", });
                        }
                    }
                }
            }

            viewProperties.ErrorMessage = "The email/password combination is incorrect.";
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmYourAccount()
        {
            var confirmModel = (ConfirmationCodeModel) TempData["ConfirmationCodeModel"];
            return View(confirmModel);
        }

        [AllowAnonymous]
        public ActionResult Confirm(string email, string code)
        {
            if (!string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(code))
            {
                if (WebSecurity.UserExists(email) &&
                    !WebSecurity.IsConfirmed(email) &&
                    WebSecurity.ConfirmAccount(email, code))
                {
                    return RedirectToAction("Login", new { SuccessMessage = "Your account has been confirmed! Please log in to start.", });
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ResendConfirmationCode(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (WebSecurity.UserExists(email) && !WebSecurity.IsConfirmed(email))
                {
                    var confirmationCodeModel = DataOperations.GetConfirmationCodeModel(email);
                    if (confirmationCodeModel != null)
                    {
                        Emailer.SendConfirmationCodeEmail(confirmationCodeModel);
                        return RedirectToAction("Login", new {SuccessMessage = "Confirmation email sent.", });
                    }
                }
            }

            throw new Exception("Error sending confirmation code email.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register(string code = null)
        {
            ViewData.Add(new KeyValuePair<string, object>("code", code));
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Invite invite = DataOperations.GetInvite(model.Email);
                    if (invite == null || invite.VerificationCode != model.InvitationCode)
                    {
                        ModelState.AddModelError("", "Invitation code is invalid.");
                    }
                    else
                    {
                        object propertyValues = new {model.FirstName, model.LastName, Timestamp = DateTime.UtcNow};
                        string confirmationCode = WebSecurity.CreateUserAndAccount(model.Email, model.Password,
                                                                                   propertyValues,
                                                                                   requireConfirmationToken: true);

                        User user = DataOperations.GetUserFromConfirmationCode(confirmationCode);
                        DataOperations.AddUserRole(user, "Dev");

                        var confirmationCodeModel = new ConfirmationCodeModel
                                                        {
                                                            Email = model.Email,
                                                            ConfirmationCode = confirmationCode,
                                                            FirstName = model.FirstName
                                                        };
                        Emailer.SendConfirmationCodeEmail(confirmationCodeModel);

                        TempData.Add("ConfirmationCodeModel", confirmationCodeModel);
                        return RedirectToAction("ConfirmYourAccount",
                                                new
                                                    {
                                                        SuccessMessage =
                                                    "Your account has been confirmed! Please log in to start.",
                                                    });
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            switch (message)
            {
                case ManageMessageId.ChangePasswordSuccess:
                    viewProperties.StatusMessage = "Your password has been changed.";
                    break;
                case ManageMessageId.RemoveLoginSuccess:
                    viewProperties.StatusMessage = "The external login was removed.";
                    break;
                case ManageMessageId.SetPasswordSuccess:
                    viewProperties.StatusMessage = "Your password has been set.";
                    break;
            }
            viewProperties.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            viewProperties.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            viewProperties.HasLocalPassword = hasLocalAccount;
            viewProperties.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                viewProperties.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                viewProperties.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel
                    {
                        UserName = result.UserName, 
                        ExternalLoginData = loginData
                    });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            string provider;
            string providerUserId;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { Email = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    
                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                }
            }

            viewProperties.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            viewProperties.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            viewProperties.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ViewProperties viewProperties = ((ViewProperties)ViewData["ViewProperties"]);
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            viewProperties.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}