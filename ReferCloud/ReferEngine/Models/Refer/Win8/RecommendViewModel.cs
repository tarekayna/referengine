using ReferLib;

namespace ReferEngineWeb.Models.Refer.Win8
{
    public class RecommendViewModel
    {
        public CurrentUser CurrentUser { get; private set; }
        public App App { get; private set; }
        public string ReferEngineAuthToken { get; set; }

        public RecommendViewModel(CurrentUser currentUser, App app, string referEngineAuthToken)
        {
            CurrentUser = currentUser;
            App = app;
            ReferEngineAuthToken = referEngineAuthToken;
        }
    }
}