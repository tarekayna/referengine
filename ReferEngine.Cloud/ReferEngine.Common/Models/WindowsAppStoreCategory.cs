using ReferEngine.Common.Utilities;

namespace ReferEngine.Common.Models
{
    public class WindowsAppStoreCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentCategoryName { get; set; }

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
                    linkPart = Util.ConvertStringToUrlPart(ParentCategoryName) + "/s/";
                }
                linkPart += Util.ConvertStringToUrlPart(Name);
                return linkPart;
            }
        }
    }
}
