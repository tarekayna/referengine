using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models
{
    public class WindowsAppStoreInfoClientSide
    {
        public WindowsAppStoreInfoClientSide(WindowsAppStoreInfo windowsAppStoreInfo)
        {
            Info = windowsAppStoreInfo;
            Link = "/appstore/windows/" + Util.ConvertStringToUrlPart(Info.Category.Name) + "/" +
                   Util.ConvertStringToUrlPart(Info.Name);
        }

        public WindowsAppStoreInfo Info { get; set; }
        public string Link { get; set; }
    }
}
