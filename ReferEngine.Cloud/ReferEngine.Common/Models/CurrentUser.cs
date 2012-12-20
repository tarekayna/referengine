using System.Collections.Generic;
using System.Runtime.Serialization;
using ReferLib;

namespace ReferEngine.Common.Models
{
    [DataContract]
    public class CurrentUser : Person
    {
        [DataMember]
        public Person Person { get; set; }

        [DataMember]
        public IList<Person> Friends { get; set; }
    }
}
