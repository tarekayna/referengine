using System.Runtime.Serialization;

namespace AppSmarts.Common.Models
{
    [DataContract]
    public class AppScreenshot
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public CloudinaryImage CloudinaryImage { get; set; }
    }
}
