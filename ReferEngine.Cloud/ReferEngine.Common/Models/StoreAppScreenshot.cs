using System;
using System.Runtime.Serialization;
using System.Web;
using System.Drawing;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class StoreAppScreenshot
    {
        [DataMember]
        public string StoreAppInfoMsAppId { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public string Caption { get; set; }
    }
}
