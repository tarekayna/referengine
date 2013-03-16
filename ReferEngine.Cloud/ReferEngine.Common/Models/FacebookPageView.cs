using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class FacebookPageView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public long AppId { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public long AppRecommendationFacebookPostId { get; set; }
    }
}
