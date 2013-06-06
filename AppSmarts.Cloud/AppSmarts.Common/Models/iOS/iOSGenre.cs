using System;
using System.Runtime.Serialization;

namespace AppSmarts.Common.Models.iOS
{
    [DataContract]
    public class iOSGenre
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime ExportDate { get; set; }

        [DataMember]
        public iOSGenre ParentGenre { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
