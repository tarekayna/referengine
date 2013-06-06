using AppSmarts.Common.Models;

namespace AppSmarts.Web.Models.Recommend.Win8
{
    public class RecommendViewModel
    {
        public Person Person { get; private set; }
        public App App { get; private set; }
        public string ReferEngineAuthToken { get; set; }
        public AppReceipt AppReceipt { get; set; }

        public RecommendViewModel(Person person, App app, string referEngineAuthToken, AppReceipt appReceipt)
        {
            Person = person;
            App = app;
            ReferEngineAuthToken = referEngineAuthToken;
            AppReceipt = appReceipt;
        }
    }
}