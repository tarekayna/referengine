namespace ReferEngine.Web.Areas.AppStore.ViewModels
{
    public class WindowsStoreSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public int PageNumber { get; set; }
        public int NumberOfApps { get; set; }
    }
}