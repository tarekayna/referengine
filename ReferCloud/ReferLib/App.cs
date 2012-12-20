using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReferLib
{
    [DataContract]
    public class App
    {
        [DataMember]
        public Int64 Id { get; set; }

        [DataMember]
        public string PackageFamilyName { get; set; }

        [DataMember]
        public string AppStoreLink { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ImageLink { get; set; }

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
        public ICollection<AppScreenshot> Screenshots { get; private set; }
    }
}