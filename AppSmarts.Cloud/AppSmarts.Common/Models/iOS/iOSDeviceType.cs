using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSDeviceType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }
    }
}
