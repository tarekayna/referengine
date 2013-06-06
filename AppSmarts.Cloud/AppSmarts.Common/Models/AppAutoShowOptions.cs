using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models
{
    [DataContract]
    public class AppAutoShowOptions
    {
        [DataMember]
        public Int64 AppId { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public int Timeout { get; set; }
    }
}
