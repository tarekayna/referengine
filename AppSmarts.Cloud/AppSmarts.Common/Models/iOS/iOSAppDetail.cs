using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSAppDetail
    {
        public iOSAppDetail()
        {
            AppScreenshots = new Collection<AppScreenshot>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSApp App { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ReleaseNotes { get; set; }

        [DataMember]
        public string CompanyUrl { get; set; }

        [DataMember]
        public string SupportUrl { get; set; }

        [DataMember]
        public ICollection<AppScreenshot> AppScreenshots { get; set; }
    }
}
