using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.ViewModels
{
    public class AppDashboardViewModel
    {
        public int NumberOfRecommendationsToShow { get { return 5; }}
        public int TotalNumberOfRecommendations { get; set; }
        public App App { get; set; }
        public IList<AppRecommendation> AppRecommendations { get; set; }
    }
}
