using System.Collections.Generic;
using System.Runtime.Serialization;
using AppSmarts.Common.Models;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.ViewModels.AppStore.Windows
{
    [DataContract]
    public class WindowsCategoryViewModel
    {
        [DataMember]
        public WindowsAppStoreCategory Category { get; set; }

        [DataMember]
        public IList<WindowsAppStoreCategory> SubCategories { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public IList<WindowsAppStoreInfo> WindowsAppStoreInfos { get; set; }

        public UserAgentProperties UserAgentProperties { get; set; }
    }
}