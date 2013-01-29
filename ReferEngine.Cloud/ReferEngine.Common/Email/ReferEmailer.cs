using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Email
{
    public static class ReferEmailer
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
                    const string username = "tarek@apexa.co";
                    const string password = "38b99f5a-45aa-4dab-8c94-452718b2afee";
                    _client = new SmtpClient(host, port);
                    _client.Credentials = new NetworkCredential(username, password);;
                }
                return _client;
            }
        }

        public static void ProcessPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            string body = "Hello! " + "\n\n";
            body += "Thank you for signing up for Refer Engine's private beta. We will let you as soon as we are ready.\n\n";
            body += "Tarek\n";
            body += "ReferEngine.com Founder and Developer";

            const string subject = "ReferEngine.com Signup";

            SendPlainTextEmail(privateBetaSignup.Email, subject, body);
        }

        public static void SendRecommendationThankYouEmail(App app, Person person)
        {
            if (!string.IsNullOrEmpty(person.Email))
            {
                StringBuilder body = new StringBuilder();
                body.AppendLine(person.FirstName != null ? "Hi " + person.FirstName + "," : "Hello!");
                body.AppendLine();
                body.AppendLine(string.Format("Thank you for recommending {0} through Refer Engine!", app.Name));
                body.AppendLine();
                body.AppendLine(
                    "Refer Engine will let you know when you earn a referral reward for this recommendation. Your recommendation will be eligible for a reward for the next 2 weeks.");
                body.AppendLine();
                body.AppendLine("Thanks :),");
                body.AppendLine("Tarek from ReferEngine.com");

                string subject = string.Format("{0} Recommendation", app.Name);
                SendPlainTextEmail(person.Email, subject, body.ToString());
            }
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
    }
}
