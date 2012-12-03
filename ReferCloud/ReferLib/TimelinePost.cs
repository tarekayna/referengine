using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class TimelinePost
    {
        public Int64 FacebookPostId { get; set; }
        public Int64 PersonFacebookId { get; set; }
        public Int64 FriendFacebookId { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
