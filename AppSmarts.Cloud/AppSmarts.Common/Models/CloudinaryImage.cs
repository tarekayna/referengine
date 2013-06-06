using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using AppSmarts.Common.Data;

namespace AppSmarts.Common.Models
{
    [DataContract]
    public class CloudinaryImage
    {
        private CloudinaryImage(){}

        public CloudinaryImage(string id, string format, string originalLink = null)
        {
            Id = id;
            Format = format;
            if (string.IsNullOrEmpty(originalLink)) return;
            OriginalLink = originalLink;
            OriginalLinkHash = GetOriginalLinkHash(originalLink);
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string OriginalLink { get; private set; }

        [DataMember]
        public string OriginalLinkHash { get; set; }

        public static string GetOriginalLinkHash(string originalLink)
        {
            HashAlgorithm algorithm = SHA256.Create();
            var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(originalLink));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        public string GetLink(string transformation = null)
        {
            return CloudinaryConnector.GetLink(this, transformation);
        }
    }
}
