using AppSmarts.Common.Models;

namespace AppSmarts.Web.Areas.AppStore.ViewModels
{
    public class WindowsStoreNavigationViewModel
    {
        public WindowsAppStoreCategory WindowsAppStoreCategory { get; set; }
        public WindowsAppStoreInfo WindowsAppStoreInfo { get; set; }
        public string SearchTerm { get; set; }
    }
}