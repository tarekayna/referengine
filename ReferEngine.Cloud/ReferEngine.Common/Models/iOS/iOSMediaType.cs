using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Utilities;
using System.Linq;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSMediaType
    {
        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
