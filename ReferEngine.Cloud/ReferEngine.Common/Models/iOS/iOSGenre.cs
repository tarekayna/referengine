using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Utilities;
using System.Linq;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSGenre
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSGenre ParentGenre { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
