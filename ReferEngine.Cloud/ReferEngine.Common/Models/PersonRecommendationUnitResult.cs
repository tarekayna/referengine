namespace ReferEngine.Common.Models
{
    public class PersonRecommendationUnitResult
    {
        public PersonRecommendationUnitResult(Person person, IpAddressLocation location, AppRecommendation recommendation)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Name = person.Name;
            PictureUrl = person.PictureUrl;
            Id = person.FacebookId;
            Gender = person.Gender;
            if (string.IsNullOrEmpty(location.Region) && location.Region != "NULL")
            {
                Location = location.City + ", " + location.Country;
            }
            else
            {
                Location = location.City + ", " + location.Region + ", " + location.Country;
            }
            UserMessage = recommendation.UserMessage;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }
        public string Location { get; set; }
        public string UserMessage { get; set; }
    }
}
