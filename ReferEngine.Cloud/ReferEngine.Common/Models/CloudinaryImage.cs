using System.Runtime.Serialization;
using ReferEngine.Common.Data;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class CloudinaryImage
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string OriginalLink { get; set; }

        public string GetLink(string transformation = null)
        {
            return CloudinaryConnector.GetLink(this, transformation);
        }
    }
}
