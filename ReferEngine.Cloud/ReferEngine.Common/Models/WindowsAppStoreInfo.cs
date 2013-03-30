using System;
using System.Collections.Generic;

namespace ReferEngine.Common.Models
{
    public class WindowsAppStoreInfo
    {
        public string Name { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string AgeRating { get; set; }
        public string Developer { get; set; }
        public string Copyright { get; set; }
        public string LogoLink { get; set; }
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
        public IEnumerable<WindowsAppStoreScreenshot> StoreAppScreenshots { get; set; }

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

        public bool IsIdentical(WindowsAppStoreInfo storeAppInfo)
        {
            return Name.Equals(storeAppInfo.Name) &&
                   Rating.Equals(storeAppInfo.Rating) &&
                   NumberOfRatings.Equals(storeAppInfo.NumberOfRatings) &&
                   Price.Equals(storeAppInfo.Price) &&
                   Category.Equals(storeAppInfo.Category) &&
                   AgeRating.Equals(storeAppInfo.AgeRating) &&
                   Developer.Equals(storeAppInfo.Developer) &&
                   Copyright.Equals(storeAppInfo.Copyright) &&
                   LogoLink.Equals(storeAppInfo.LogoLink) &&
                   DescriptionHtml.Equals(storeAppInfo.DescriptionHtml) &&
                   FeaturesHtml.Equals(storeAppInfo.FeaturesHtml) &&
                   WebsiteLink.Equals(storeAppInfo.WebsiteLink) &&
                   SupportLink.Equals(storeAppInfo.SupportLink) &&
                   PrivacyPolicyLink.Equals(storeAppInfo.PrivacyPolicyLink) &&
                   ReleaseNotes.Equals(storeAppInfo.ReleaseNotes) &&
                   Architecture.Equals(storeAppInfo.Architecture) &&
                   Languages.Equals(storeAppInfo.Languages) &&
                   MsAppId.Equals(storeAppInfo.MsAppId) &&
                   PackageFamilyName.Equals(storeAppInfo.PackageFamilyName);
        }
    }
}
