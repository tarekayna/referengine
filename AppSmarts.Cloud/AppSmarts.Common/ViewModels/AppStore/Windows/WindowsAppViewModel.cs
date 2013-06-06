using System.Runtime.Serialization;
using AppSmarts.Common.Models;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.ViewModels.AppStore.Windows
{
    [DataContract]
    public class WindowsAppViewModel
    {
        [DataMember]
        public App App { get; set; }

        [DataMember]
        public int NumberOfRecommendations { get; set; }

        public UserAgentProperties UserAgentProperties { get; set; }

        [DataMember]
        public WindowsAppStoreInfo WindowsAppStoreInfo { get; set; }
    }
}