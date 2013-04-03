using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Email
{
    public static class Emailer
    {
        private static readonly MailAddress From = new MailAddress("tarek@referengine.com", "Tarek, ReferEngine.com");
        
        private static SmtpClient _client;
        private static SmtpClient Client
        {
            get
            {
                if (_client == null)
                {
                    const string host = "smtp.mandrillapp.com";
                    const int port = 587;
                    const string username = "tarek@referengine.com";
                    const string password = "38b99f5a-45aa-4dab-8c94-452718b2afee";
                    _client = new SmtpClient(host, port);
                    _client.Credentials = new NetworkCredential(username, password);;
                }
                return _client;
            }
        }

        public static void ProcessPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("Hi,");
            body.AppendLine();
            body.AppendLine("Thank you for requesting an invite to Refer Engine's private beta.");
            body.AppendLine("I will contact you soon.");
            body.AppendLine();
            body.AppendLine("Tarek,");
            body.AppendLine("Founder and Developer, ReferEngine.com");

            const string subject = "ReferEngine.com Invite Request";

            SendPlainTextEmail(privateBetaSignup.Email, subject, body.ToString());
        }

        public static void SendRecommendationThankYouEmail(App app, Person person)
        {
            if (app == null || person == null) return;
            if (string.IsNullOrEmpty(person.Email)) return;
            StringBuilder body = new StringBuilder();
            body.AppendLine(person.FirstName != null ? "Hi " + person.FirstName + "," : "Hello!");
            body.AppendLine();
            body.AppendLine(string.Format("Thank you for recommending {0} through Refer Engine!", app.Name));
            body.AppendLine();
            body.AppendLine("We appreciate your support for your favorite apps.");
            body.AppendLine();
            body.AppendLine("Thanks :),");
            body.AppendLine("Tarek,");
            body.AppendLine("Founder and Developer, ReferEngine.com");

            string subject = string.Format("{0} Recommendation", app.Name);
            SendPlainTextEmail(person.Email, subject, body.ToString());
        }

        public static void SendInviteEmail(Invite invite)
        {
            if (invite == null) return;
            StringBuilder body = new StringBuilder();
            body.AppendLine("Hi,");
            body.AppendLine();
            body.AppendLine(string.Format("Congratulations! you are invited to the private beta of Refer Engine."));
            body.AppendLine("It should take you under 20 minutes to register, add your app and integrate Refer Engine.");
            body.AppendLine();
            body.AppendLine("Registration Link: https://www.ReferEngine.com/account/register?code=" + invite.VerificationCode);
            body.AppendLine();
            body.AppendLine("Invite Code: " + invite.VerificationCode);
            body.AppendLine();
            body.AppendLine("I'm looking forward to working with you. If you have any questions or issues please contact me.");
            body.AppendLine();
            body.AppendLine("Tarek,");
            body.AppendLine("Founder and Developer, ReferEngine.com");

            string subject = string.Format("Invitation to Refer Engine's Private Beta");
            SendPlainTextEmail(invite.Email, subject, body.ToString());
        }

        public static void SendConfirmationCodeEmail(ConfirmationCodeModel model)
        {
            if (model == null) return;

            StringBuilder body = new StringBuilder();
            body.AppendLine("Hi " + model.FirstName + ",");
            body.AppendLine();
            body.AppendLine(string.Format("Thank you for registering at ReferEngine.com!"));
            body.AppendLine();
            body.AppendLine(
                "Before you get started, you need to confirm your email account. You either use the link below or enter your confirmation" +
                "code after you login.");
            body.AppendLine();
            body.AppendLine(string.Format("Confirmation Code: {0}", model.ConfirmationCode));
            body.AppendLine();
            body.AppendLine(string.Format("https://www.referengine.com/account/confirm?email={0}&code={1}", model.Email, model.ConfirmationCode));
            body.AppendLine();
            body.AppendLine("Thanks :),");
            body.AppendLine("Tarek from ReferEngine.com");

            string subject = string.Format("ReferEngine: Please Confirm Registration");
            SendPlainTextEmail(model.Email, subject, body.ToString());
        }

        public static void SendPlainTextEmail(string to, string subject, string body)
        {
            var toMailAddress = new MailAddress(to);
            var mailMessage = new MailMessage(From, toMailAddress)
            {
                Body = body,
                IsBodyHtml = false,
                Sender = From,
                Subject = subject
            };
            mailMessage.ReplyToList.Add(From);
            Client.Send(mailMessage);
        }

        public static void SendWindowsAppInvite(string to, WindowsAppStoreInfo appInfo, Invite invite, string name = null)
        {
            const string template = "refer-engine-windows-app-invitation";
            string subject = appInfo.Name + "'s Private Beta Invite to Refer Engine";

            dynamic vars = new
                {
                    _rcpt = to,
                    Greeting = name == null ? "Hi" : "Hi " + name,
                    InviteLink = "https://www.ReferEngine.com/account/register?code=" + invite.VerificationCode,
                    InviteCode = invite.VerificationCode,
                    AppName = appInfo.Name
                };

            string mergeVars = Json.Encode(vars);

            SendTemplateEmail(template, to, subject, mergeVars);
        }

        public static void SendTemplateEmail(string template, string to, string subject, string mergeVars)
        {
            var toMailAddress = new MailAddress(to);
            var mailMessage = new MailMessage(From, toMailAddress)
            {
                Sender = From,
                Subject = subject
            };
            mailMessage.Headers.Add("X-MC-MergeVars", mergeVars);
            mailMessage.Headers.Add("X-MC-Template", template);
            mailMessage.ReplyToList.Add(From);
            Client.Send(mailMessage);
        }

        public static void SendExceptionEmail(Exception e, string subject = null)
        {
            StringBuilder builder = new StringBuilder();
            Exception ex = e;
            while (ex != null)
            {
                builder.AppendLine(e.Message);
                builder.AppendLine();
                builder.AppendLine(e.StackTrace);
                builder.AppendLine();
                builder.AppendLine();
                ex = ex.InnerException;
            }

            SendPlainTextEmail("tarek@referengine.com", subject ?? "Exception", builder.ToString());
        }
    }
}
