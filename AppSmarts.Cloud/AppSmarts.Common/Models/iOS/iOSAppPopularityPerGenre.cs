using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSAppPopularityPerGenre
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSStorefront Storefront { get; set; }

        [DataMember]
        public iOSGenre Genre { get; set; }

        [DataMember]
        public iOSApp App { get; set; }

        [DataMember]
        public int Rank { get; set; }
    }
}
