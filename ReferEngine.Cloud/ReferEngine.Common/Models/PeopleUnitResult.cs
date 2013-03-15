namespace ReferEngine.Common.Models
{
    public class PeopleRecommendationUnitResult
    {
        public Person Person { get; set; }
        public IpAddressLocation IpAddressLocation { get; set; }
        public AppRecommendation AppRecommendation { get; set; }
    }
}
