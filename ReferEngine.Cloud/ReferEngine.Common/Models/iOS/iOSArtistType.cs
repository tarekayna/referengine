using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSArtistType
    {
        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public iOSMediaType MediaType { get; set; }
    }
}
