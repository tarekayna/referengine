using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class Friendship
    {
        public Int64 Id { get; set; }
        public Int64 Person1FacebookId { get; set; }
        public Int64 Person2FacebookId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public Friendship(Person person1, Person person2)
        {
            Person1FacebookId = person1.FacebookId;
            Person2FacebookId = person2.FacebookId;
            UpdatedDateTime = DateTime.Now;
        }
    }
}
