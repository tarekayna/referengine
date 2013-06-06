using System;
using System.Security.Cryptography;
using System.Text;

namespace AppSmarts.Common.Models
{
    public class Invite
    {
        public string Email { get; set; }
        public bool IsRequested { get; set; }
        public string MsAppId { get; set; }
        public string VerificationCode { get; set; }
        public DateTime TimeStamp { get; set; }

        public static Invite Generate(string email, bool isRequested = true, string msAppId = null)
        {
            Invite invite = new Invite { Email = email, TimeStamp = DateTime.UtcNow };
            invite.ComputeVerificationCode();
            return invite;
        }

        private void ComputeVerificationCode()
        {
            string str = String.Format("{0}{1}", Email, TimeStamp);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (HMACSHA256 sha = new HMACSHA256())
            {
                sha.Initialize();
                byte[] hashBytes = sha.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                VerificationCode = builder.ToString();
            }
        }
    }
}
