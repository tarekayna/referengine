using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models
{
    [DataContract]
    public class PrivateBetaSignup
    {
        public PrivateBetaSignup() {}

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string AppName { get; set; }

        [DataMember]
        public string Platforms { get; set; }

        [DataMember]
        public DateTime RegistrationDateTime { get; set; }

        public PrivateBetaSignup(string email)
        {
            Email = email;
            RegistrationDateTime = DateTime.UtcNow;
        }
    }
}
