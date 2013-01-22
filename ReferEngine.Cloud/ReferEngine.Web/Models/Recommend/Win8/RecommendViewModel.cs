using ReferEngine.Common.Models;

namespace ReferEngine.Web.Models.Recommend.Win8
{
    public class RecommendViewModel
    {
        public CurrentUser CurrentUser { get; private set; }
        public App App { get; private set; }
        public string ReferEngineAuthToken { get; set; }
        public AppReceipt AppReceipt { get; set; }

        public RecommendViewModel(CurrentUser currentUser, App app, string referEngineAuthToken, AppReceipt appReceipt)
        {
            CurrentUser = currentUser;
            App = app;
            ReferEngineAuthToken = referEngineAuthToken;
            AppReceipt = appReceipt;
        }
    }
}