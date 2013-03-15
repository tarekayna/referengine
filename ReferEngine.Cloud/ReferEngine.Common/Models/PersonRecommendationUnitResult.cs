using System;
using ReferEngine.Common.Data;

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

            if (person.NumberOfFriends != 0)
            {
                ToFriends = " to ";
                ToFriends += Gender == "male" ? "his " : "her ";
                ToFriends += person.NumberOfFriends + " friends";
            }

            if (location != null)
            {
                if (string.IsNullOrEmpty(location.Region) || location.Region == "NULL")
                {
                    Location = location.City + ", " + location.Country;
                }
                else
                {
                    Location = location.City + ", " + location.Region + ", " + location.Country;
                }
            }
            UserMessage = recommendation.UserMessage;

            DateTime now = DateTime.UtcNow;
            TimeSpan span = now - recommendation.DateTime;
            if (span < TimeSpan.FromMinutes(1))
            {
                Time = "around " + span.Seconds + " seconds ago";
            }
            if (span < TimeSpan.FromHours(1))
            {
                Time = "around " + span.Minutes + " minutes ago";
            }
            else if (span < TimeSpan.FromDays(1))
            {
                Time = "around " + span.Hours + " hours ago";
            }
            else
            {
                Time = "around " + span.Days + " days ago";
            }
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }
        public string ToFriends { get; set; }
        public string Location { get; set; }
        public string UserMessage { get; set; }
        public string Time { get; set; }
    }
}
