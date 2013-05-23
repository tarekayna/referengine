using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSAppArtist
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSArtist Artist { get; set; }

        [DataMember]
        public iOSApp App { get; set; }
    }
}
