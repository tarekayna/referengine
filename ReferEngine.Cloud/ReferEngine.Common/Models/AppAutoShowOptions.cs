using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class AppAutoShowOptions
    {
        [DataMember]
        public Int64 AppId { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public int Timeout { get; set; }
    }
}
