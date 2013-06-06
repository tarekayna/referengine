using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSApp
    {
        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string RecommendedAge { get; set; }

        [DataMember]
        public string ArtistName { get; set; }

        [DataMember]
        public string SellerName { get; set; }

        [DataMember]
        public string CompanyUrl { get; set; }

        [DataMember]
        public string SupportUrl { get; set; }

        [DataMember]
        public string ViewUrl { get; set; }

        [DataMember]
        public CloudinaryImage ArtworkLarge { get; set; }

        [DataMember]
        public CloudinaryImage ArtworkSmall { get; set; }

        [DataMember]
        public DateTime iTunesReleaseDate { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string iTunesVersion { get; set; }

        [DataMember]
        public Int64 DownloadSize { get; set; }
    }
}
