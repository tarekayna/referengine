using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class PrivateBetaSignup
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime RegistrationDateTime { get; set; }

        public PrivateBetaSignup(string email)
        {
            Email = email;
            RegistrationDateTime = DateTime.UtcNow;
        }
    }
}
