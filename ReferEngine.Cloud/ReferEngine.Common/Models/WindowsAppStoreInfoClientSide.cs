using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReferEngine.Common.Utilities;
using System.Linq;

namespace ReferEngine.Common.Models
{
    public class WindowsAppStoreInfoClientSide
    {
        public WindowsAppStoreInfoClientSide(WindowsAppStoreInfo windowsAppStoreInfo)
        {
            Link = "/app-store/windows/" + windowsAppStoreInfo.LinkPart;
            AppName = windowsAppStoreInfo.Name;
            BackgroundColor = windowsAppStoreInfo.GetBackgroundColor();

            if (windowsAppStoreInfo.LogoImage != null)
            {
                LogoLink = windowsAppStoreInfo.LogoImage.GetLink();
            }

            if (windowsAppStoreInfo.AppScreenshots != null && windowsAppStoreInfo.AppScreenshots.Any())
            {
                ScreenshotLink = windowsAppStoreInfo.AppScreenshots.First().CloudinaryImage.GetLink("h_120,w_370,c_fill");
            }
        }

        public string Link { get; set; }
        public string BackgroundColor { get; set; }
        public string LogoLink { get; set; }
        public string ScreenshotLink { get; set; }
        public string AppName { get; set; }
    }
}
