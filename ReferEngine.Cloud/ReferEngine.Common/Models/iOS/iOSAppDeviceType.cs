using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.iOS;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models.iOS
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
