using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.ViewModels.AppStore.Windows
{
    [DataContract]
    public class WindowsAppViewModel
    {
        [DataMember]
        public App App { get; private set; }

        [DataMember]
        public int NumberOfRecommendations { get; set; }

        public UserAgentProperties UserAgentProperties { get; set; }

        [DataMember]
        public WindowsAppStoreInfo WindowsAppStoreInfo { get; private set; }

        public WindowsAppViewModel(WindowsAppStoreInfo appInfo, App app)
        {
            WindowsAppStoreInfo = appInfo;
            App = app;
        }
    }
}