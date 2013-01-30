using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.Models.AppModels
{
    public class AppDashboardViewModel
    {
        public int NumberOfRecommendationsToShow { get { return 5; } }

        public AppDashboardViewModel(App app)
        {
            User = DatabaseOperations.GetUser(app.UserId);
            App = app;
            AppRecommendations = DatabaseOperations.GetAppRecommdations(app, NumberOfRecommendationsToShow);
        }

        public App App { get; set; }
        public IList<AppRecommendation> AppRecommendations { get; set; }
        public User User { get; set; }
    }
}