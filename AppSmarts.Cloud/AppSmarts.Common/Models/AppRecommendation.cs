using System;
using AppSmarts.Common.Data;
using AppSmarts.Common.Email;

namespace AppSmarts.Common.Models
{
    public class AppRecommendation
    {
        public Int64 AppId { get; set; }
        public DateTime DateTime { get; set; }
        public Int64 FacebookPostId { get; set; }
        public Int64 PersonFacebookId { get; set; }
        public string UserMessage { get; set; }
        public string IpAddress { get; set; }

        public AppRecommendation()
        {
            DateTime = DateTime.UtcNow;
        }

        public static void ProcessNew(AppRecommendation recommendation)
        {
            DatabaseOperations.AddRecommendation(recommendation);

            App app = DatabaseOperations.GetApp(recommendation.AppId);
            Person person = DatabaseOperations.GetPerson(recommendation.PersonFacebookId);
            Emailer.SendRecommendationThankYouEmail(app, person);            
        }
    }
}
