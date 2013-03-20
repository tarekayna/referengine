using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class App
    {
        public App() {}

        [DataMember]
        public Int64 Id { get; set; }
        
        [DataMember]
        public string PackageFamilyName { get; set; }

        [DataMember]
        public string AppStoreLink { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LogoLink50 { get; set; }

        [DataMember]
        public string LogoLinkHighQuality { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string VerificationCode { get; set; }

        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public string VimeoLink { get; set; }

        [DataMember]
        public string YouTubeLink { get; set; }

        [DataMember]
        public string Platform { get; set; }

        [DataMember]
        public AppRewardPlan RewardPlan { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public List<AppScreenshot> Screenshots { get; set; }

        public void ComputeVerificationCode()
        {
            string str = String.Format("{0}{1}{2}", Id, UserId, DateTime.Now.Ticks);
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