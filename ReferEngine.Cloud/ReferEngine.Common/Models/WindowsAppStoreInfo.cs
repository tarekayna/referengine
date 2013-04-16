using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class WindowsAppStoreInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Rating { get; set; }

        [DataMember]
        public int NumberOfRatings { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public WindowsAppStoreCategory Category { get; set; }

        [DataMember]
        public string AgeRating { get; set; }

        [DataMember]
        public string Developer { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public string DescriptionHtml { get; set; }

        [DataMember]
        public string FeaturesHtml { get; set; }

        [DataMember]
        public string WebsiteLink { get; set; }

        [DataMember]
        public string SupportLink { get; set; }

        [DataMember]
        public string PrivacyPolicyLink { get; set; }

        [DataMember]
        public string ReleaseNotes { get; set; }

        [DataMember]
        public string Architecture { get; set; }

        [DataMember]
        public string Languages { get; set; }

        [DataMember]
        public string MsAppId { get; set; }

        [DataMember]
        public string PackageFamilyName { get; set; }

        [DataMember]
        public string AppStoreLink { get; set; }

        [DataMember]
        public string BackgroundColor { get; set; }

        [DataMember]
        public CloudinaryImage LogoImage { get; set; }

        [DataMember]
        public virtual ICollection<CloudinaryImage> CloudinaryImages { get; private set; }

        public string LinkPart
        {
            get
            {
                string linkPart = Category.LinkPart + "/";
                linkPart += Util.ConvertStringToUrlPart(Name);
                return linkPart;
            }
        }

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
