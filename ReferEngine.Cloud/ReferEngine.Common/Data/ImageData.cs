using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ReferEngine.Common.Data
{
    public class ImageData
    {
        private static Account _cloudinaryAccount;
        private static Account CloudinaryAccount
        {
            get
            {
                return _cloudinaryAccount ??
                       (_cloudinaryAccount = new Account("hrwr3ufuu", "662139491268389", "k5lnrUmOg3G3Yc9Z7lTYysljjG4"));
            }
        }

        private static Cloudinary _cloudinary;
        private static Cloudinary Cloudinary
        {
            get { return _cloudinary ?? (_cloudinary = new Cloudinary(CloudinaryAccount)); }
        }

        public static string BaseLink { get { return "http://res.cloudinary.com/hrwr3ufuu/image/upload/"; } }

        public static string GetAppBackgroundImage(string name, int height, int width)
        {
            return Cloudinary.Api.UrlImgUp
                                 .Transform(new Transformation().Width(width).Height(height).Crop("fill").Named("Blur"))
                                 .BuildUrl(name); 
        }

        public static ImageUploadResult UploadRemote(string url, string name)
        {
            ImageUploadParams uploadParams = new ImageUploadParams {File = new FileDescription(url), PublicId = name};

            return Cloudinary.Upload(uploadParams);
        }

        public static string GetLink(string name, string transformation = null)
        {
            string link = BaseLink;
            link += string.IsNullOrEmpty(transformation) ? "" : transformation + "/";
            link += name;
            return link;
        }
    }
}
