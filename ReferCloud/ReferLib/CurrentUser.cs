using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class CurrentUser : Person
    {
        public Person Person { get; set; }
        public IList<Person> Friends { get; set; }
    }
}
