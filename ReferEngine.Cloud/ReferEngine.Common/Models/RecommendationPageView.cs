using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    public enum RecommendationPage
    {
        Intro = 0,
        Post,
        AppLaunch,
    }

    [DataContract]
    public class RecommendationPageView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string AppReceiptId { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public RecommendationPage RecommendationPage { get; set; }

        [DataMember]
        public Int64 AppId { get; set; }

        [DataMember]
        public bool IsAutoOpen { get; set; }

        [DataMember]
        public string IpAddress { get; set; }
    }
}
