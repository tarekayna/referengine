using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSDeviceType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }
    }
}
