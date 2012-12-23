using System;

namespace ReferEngine.Common.Models
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
