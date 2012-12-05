using System;

namespace ReferLib
{
    public class AppRecommendation
    {
        public Int64 AppId { get; set; }
        public DateTime DateTime { get; set; }
        public Int64 FacebookPostId { get; set; }
        public Int64 PersonFacebookId { get; set; }

        public AppRecommendation(Int64 appId, Int64 facebookPostId, Int64 personFacebookId)
        {
            this.AppId = appId;
            this.FacebookPostId = facebookPostId;
            this.PersonFacebookId = personFacebookId;
            DateTime = DateTime.Now;
        }
    }
}
