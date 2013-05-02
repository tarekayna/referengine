using System.Runtime.Serialization;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class WindowsAppStoreCategory
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ParentCategoryName { get; set; }

        [DataMember]
        public CloudinaryImage CloudinaryImage { get; set; }

        public bool HasParent
        {
            get { return !string.IsNullOrEmpty(ParentCategoryName); }
        }

        public string LinkPart
        {
            get
            {
                string linkPart = string.Empty;
                if (HasParent)
                {
                    linkPart = "s/" + Util.ConvertStringToUrlPart(ParentCategoryName) + "/";
                }
                linkPart += Util.ConvertStringToUrlPart(Name);
                return linkPart;
            }
        }

        public string Schema
        {
            get
            {
                return Util.GetCategorySchema(HasParent ? ParentCategoryName : Name);
            }
        }
    }
}
