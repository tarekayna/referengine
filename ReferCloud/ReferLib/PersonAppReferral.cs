using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class PersonAppReferral
    {
        public int PersonFacebookId { get; set; }
        public int AppId { get; set; }
        public DateTime ReferralDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }
    }
}
