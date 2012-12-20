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
        public string Id { get; private set; }

        [DataMember]
        public Int64 AppId { get; private set; }

        [DataMember]
        public string AppPackageFamilyName { get; private set; }

        [DataMember]
        public DateTime PurchaseDate { get; private set; }

        [DataMember]
        public LicenseType LicenseType { get; private set; }

        [DataMember]
        public Int64 PersonFacebookId { get; set; }

        public AppReceipt(string id, Int64 appId, string appPackageFamilyName, DateTime purchaseDate, LicenseType licenseType)
        {
            AppId = appId;
            AppPackageFamilyName = appPackageFamilyName;
            Id = id;
            PurchaseDate = purchaseDate;
            LicenseType = licenseType;
        }

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
