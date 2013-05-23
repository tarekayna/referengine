using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Models;
using ReferEngine.Common.Models.iOS;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.ViewModels.AppStore.iOS
{
    [DataContract]
    public class iOSAppViewModel
    {
        public UserAgentProperties UserAgentProperties { get; set; }

        [DataMember]
        public iOSApp App { get; set; }

        [DataMember]
        public iOSAppDetail AppDetail { get; set; }
    }
}