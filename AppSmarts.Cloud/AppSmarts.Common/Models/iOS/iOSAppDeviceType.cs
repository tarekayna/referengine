using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSAppDeviceType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSApp App { get; set; }

        [DataMember]
        public iOSDeviceType DeviceType { get; set; }
    }
}
