using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReferEngine.Common.Models
{
    public class WindowsAppStoreInfo
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public double Price { get; set; }
        public WindowsAppStoreCategory Category { get; set; }
        public string AgeRating { get; set; }
        public string Developer { get; set; }
        public string Copyright { get; set; }
        public string DescriptionHtml { get; set; }
        public string FeaturesHtml { get; set; }
        public string WebsiteLink { get; set; }
        public string SupportLink { get; set; }
        public string PrivacyPolicyLink { get; set; }
        public string ReleaseNotes { get; set; }
        public string Architecture { get; set; }
        public string Languages { get; set; }
        public string MsAppId { get; set; }
        public string PackageFamilyName { get; set; }
        public string AppStoreLink { get; set; }
        public string BackgroundColor { get; set; }
        public CloudinaryImage LogoImage { get; set; }
        public virtual ICollection<CloudinaryImage> CloudinaryImages { get; private set; }

        public WindowsAppStoreInfo()
        {
            CloudinaryImages = new Collection<CloudinaryImage>();
        }

        public void SetRating(string starRatingText)
        {
            if (string.IsNullOrEmpty(starRatingText))
            {
                Rating = 0;
            }
            else
            {
                int end = starRatingText.IndexOf("out", StringComparison.OrdinalIgnoreCase) - 1;
                int start = "Rating: ".Length;
                string rating = starRatingText.Substring(start, end - start);
                Rating = Convert.ToDouble(rating);
            }
        }

        public void SetNumberOfRatings(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Equals("No rating", StringComparison.OrdinalIgnoreCase))
            {
                NumberOfRatings = 0;
            }
            else
            {
                int end = text.IndexOf(" rating", StringComparison.OrdinalIgnoreCase);
                string num = text.Substring(0, end);
                NumberOfRatings = Convert.ToInt32(num);
            }
        }

        public void SetPrice(string priceText)
        {
            if (string.IsNullOrEmpty(priceText))
            {
                Price = 0;
            }
            else
            {
                Price = priceText.IndexOf("Free", StringComparison.OrdinalIgnoreCase) != -1
                            ? 0
                            : Convert.ToDouble(priceText.Substring(priceText.IndexOf("$") + 1));
            }
        }

        public void Update(WindowsAppStoreInfo info)
        {
            Name = info.Name;
            Rating = info.Rating;
            NumberOfRatings = info.NumberOfRatings;
            Price = info.Price;
            Category = info.Category;
            AgeRating = info.AgeRating;
            Developer = info.Developer;
            Copyright = info.Copyright;
            DescriptionHtml = info.DescriptionHtml;
            FeaturesHtml = info.FeaturesHtml;
            WebsiteLink = info.WebsiteLink;
            SupportLink = info.SupportLink;
            PrivacyPolicyLink = info.PrivacyPolicyLink;
            ReleaseNotes = info.ReleaseNotes;
            Architecture = info.Architecture;
            Languages = info.Languages;
            PackageFamilyName = info.PackageFamilyName;
            AppStoreLink = info.AppStoreLink;
            BackgroundColor = info.BackgroundColor;
        }
    }
}
