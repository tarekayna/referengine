using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSAppPrice
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSApp App { get; set; }

        [DataMember]
        public string RetailPrice { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        [DataMember]
        public iOSStorefront Storefront { get; set; }
    }
}
