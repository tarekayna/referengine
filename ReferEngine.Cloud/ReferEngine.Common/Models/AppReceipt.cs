using System;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    public enum LicenseType
    {
        Full = 0,
        Trial = 1
    }
    
    [DataContract]
    public class AppReceipt
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string CertificateId { get; set; }

        [DataMember]
        public Int64 AppId { get; set; }

        [DataMember]
        public string AppPackageFamilyName { get; set; }

        [DataMember]
        public DateTime PurchaseDate { get; set; }

        [DataMember]
        public LicenseType LicenseType { get; set; }

        [DataMember]
        public Int64 PersonFacebookId { get; set; }

        [DataMember]
        public string XmlContent { get; set; }

        [DataMember]
        public bool Verified { get; set; }

        public static LicenseType GetLicenseType(string licenseTypeStr)
        {
            if (licenseTypeStr.Equals("Full", StringComparison.OrdinalIgnoreCase))
            {
                return LicenseType.Full;
            }
            if (licenseTypeStr.Equals("Trial", StringComparison.OrdinalIgnoreCase))
            {
                return LicenseType.Trial;
            }

            throw new InvalidOperationException();
        }
    }
}
