using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public sealed class App
    {
        public App()
        {
            if (Screenshots == null)
            {
                Screenshots = new Collection<AppScreenshot>();
            }
        }

        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public string PackageFamilyName { get; set; }

        [DataMember]
        public string AppStoreLink { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LogoLink50 { get; set; }

        [DataMember]
        public string LogoLink150 { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public Int64 DeveloperId { get; set; }

        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public string VimeoLink { get; set; }

        [DataMember]
        public string YouTubeLink { get; set; }

        private ICollection<AppScreenshot> _screenshots;

        public ICollection<AppScreenshot> Screenshots
        {
            get { return _screenshots ?? (_screenshots = new Collection<AppScreenshot>()); }
            set { _screenshots = value; }
        }
    }
}