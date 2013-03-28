using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models
{
    public class Invite
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public DateTime TimeStamp { get; set; }

        public static Invite Generate(string email)
        {
            Invite invite = new Invite { Email = email, TimeStamp = DateTime.UtcNow };
            invite.ComputeVerificationCode();
            return invite;
        }

        public void ComputeVerificationCode()
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
