using System;
using System.Runtime.Serialization;
using System.Web;
using System.Drawing;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class AppScreenshot
    {
        private const int MaxFileSize = 5242880;
        private const string MaxFileSizeStr = "5MB";

        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public Int64 AppId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Size { get; set; }

        public string Link
        {
            get { return string.Format("https://referenginestorage.blob.core.windows.net/app-screenshots/{0}.png", Id); }
        }

        public static AppScreenshot Create(HttpPostedFileBase file, int appId)
        {
            if (file.ContentLength == 0)
            {
                throw new Exception("File is empty");
            }
            if (file.ContentLength > MaxFileSize)
            {
                throw new Exception("File must be smaller than " + MaxFileSizeStr);
            }
            if (!file.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("File must be a .png image");
            }

            Image image = Image.FromStream(file.InputStream);

            AppScreenshot appScreenshot = new AppScreenshot
            {
                Description = "sample desc",
                AppId = appId,
                Height = image.Height,
                Width = image.Width,
                Size = file.ContentLength
            };

            return appScreenshot;
        }
    }
}
