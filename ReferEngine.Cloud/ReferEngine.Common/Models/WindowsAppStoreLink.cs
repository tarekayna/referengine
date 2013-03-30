using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class WindowsAppStoreLink
    {
        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }
    }
}
