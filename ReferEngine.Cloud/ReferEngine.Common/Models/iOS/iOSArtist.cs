using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models.iOS
{
    [DataContract]
    public class iOSArtist
    {
        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SearchTerms { get; set; }

        [DataMember]
        public bool IsActualArtist { get; set; }

        [DataMember]
        public string ViewUrl { get; set; }

        [DataMember]
        public iOSArtistType ArtistType { get; set; }
    }
}
