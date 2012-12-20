using System;

namespace ReferLib
{
    public class AppRecommendation
    {
        public Int64 AppId { get; set; }
        public DateTime DateTime { get; set; }
        public Int64 FacebookPostId { get; set; }
        public Int64 PersonFacebookId { get; set; }
        public string UserMessage { get; set; }

        public AppRecommendation()
        {
            DateTime = DateTime.Now;
        }
    }
}
