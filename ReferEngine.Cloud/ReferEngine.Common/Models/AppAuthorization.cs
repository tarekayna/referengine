using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class AppAuthorization
    {
        [DataMember]
        public App App { get; set; }

        [DataMember]
        public AppReceipt AppReceipt { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string UserHostAddress { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        private string _privateKey = "fo435u49ftrl93455498634jrlkenrfp3woijrewrjewlkrjewjrwlerew";

        public AppAuthorization(App app, AppReceipt appReceipt, string userHostAddress)
        {
            App = app;
            AppReceipt = appReceipt;
            UserHostAddress = userHostAddress;
            Token = ComputeToken();
            IsVerified = false;
        }

        private string ComputeToken()
        {
            string str = String.Format("{0}{1}{2}{3}{4}", _privateKey, AppReceipt.Id, App.Id, App.DeveloperId,
                                       AppReceipt.PurchaseDate);
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
                return builder.ToString();
            }
        }
    }
}
