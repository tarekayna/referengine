using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ReferEngine.Common.Models;
using ImageInfo = ReferEngine.Common.Models.ImageInfo;

namespace ReferEngine.Common.Data
{
    internal class CloudinaryConnector
    {
        private static Account _cloudinaryAccount;
        private static Account CloudinaryAccount
        {
            get
            {
                return _cloudinaryAccount ??
                       (_cloudinaryAccount = new Account("referengine", "945283732626525", "HAobnQaoWPS2ECMTXP2iWO_84I8"));
            }
        }

        private static Cloudinary _cloudinary;
        private static Cloudinary Cloudinary
        {
            get { return _cloudinary ?? (_cloudinary = new Cloudinary(CloudinaryAccount)); }
        }

        private static string _baseUrl = "https://cloudinary-a.akamaihd.net/referengine/image/upload/";

        public static string GetAppBackgroundImage(string name, int height, int width)
        {
            return Cloudinary.Api.UrlImgUp
                                 .Transform(new Transformation().Width(width).Height(height).Crop("fill").Named("Blur"))
                                 .BuildUrl(name); 
        }

        public static void DeleteImage(CloudinaryImage cloudinaryImage)
        {
            Cloudinary.DeleteResources(cloudinaryImage.Id);
        }

        public static CloudinaryImage UploadRemoteImage(ImageInfo imageInfo, string tags = null)
        {
            string t = string.IsNullOrEmpty(tags) ? Utilities.Util.CurrentServiceConfigurationString : tags + Utilities.Util.CurrentServiceConfigurationString;
            ImageUploadParams uploadParams = new ImageUploadParams
                                                 {
                                                     File = new FileDescription(imageInfo.Link),
                                                     UseFilename = false,
                                                     Tags = t
                                                 };
            ImageUploadResult uploadResult = Cloudinary.Upload(uploadParams);
            CloudinaryImage cloudinaryImage = new CloudinaryImage
                                                  {
                                                      Id = uploadResult.PublicId,
                                                      OriginalLink = imageInfo.Link,
                                                      Description = imageInfo.Description,
                                                      Format = uploadResult.Format
                                                  };
            return cloudinaryImage;
        }

        public static string GetLink(CloudinaryImage cloudinaryImage, string transformation = null)
        {
            string link = _baseUrl;
            if (!string.IsNullOrEmpty(transformation))
            {
                link += transformation + "/";
            }
            link += cloudinaryImage.Id + "." + cloudinaryImage.Format;
            return link;
        }
    }
}
