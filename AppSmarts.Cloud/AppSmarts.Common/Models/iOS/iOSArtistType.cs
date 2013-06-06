using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
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
