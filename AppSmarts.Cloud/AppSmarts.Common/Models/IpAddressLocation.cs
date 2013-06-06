using System.Runtime.Serialization;

namespace AppSmarts.Common.Models
{
    [DataContract]
    public class IpAddressLocation
    {
        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public short Confidence { get; set; }

        [DataMember]
        public string Results { get; set; }

        [DataMember]
        public string Domain { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string ISP { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CountryAbbreviation { get; set; }
    }
}