namespace AppSmarts.Web.Models.Common
{
    public class PageTitleViewModel
    {
        public PageTitleViewModel(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        public string PageTitle { get; set; }
    }
}