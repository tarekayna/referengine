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

        private static void SendPlainTextEmail(string to, string subject, string body)
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
