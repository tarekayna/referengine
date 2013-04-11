using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.ViewModels.AppStore.Windows
{
    [DataContract]
    public class WindowsCategoryViewModel
    {
        [DataMember]
        public WindowsAppStoreCategory Category { get; set; }

        [DataMember]
        public IList<WindowsAppStoreCategory> SubCategories { get; set; }

        [DataMember]
        public IList<WindowsAppStoreInfo> AppStoreInfos { get; set; }

        public UserAgentProperties UserAgentProperties { get; set; }
    }
}