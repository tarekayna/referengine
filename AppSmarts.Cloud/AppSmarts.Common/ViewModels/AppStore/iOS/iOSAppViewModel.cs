using System.Runtime.Serialization;
using AppSmarts.Common.Models.iOS;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.ViewModels.AppStore.iOS
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