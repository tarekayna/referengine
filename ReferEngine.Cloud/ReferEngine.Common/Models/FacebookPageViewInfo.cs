using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class FacebookPageViewInfo
    {
        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public long AppId { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string ActionId { get; set; }
    }
}
