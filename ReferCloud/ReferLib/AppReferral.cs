using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferLib
{
    public class AppReferral
    {
        public Int64 AppId { get; set; }
        public DateTime DateTime { get; set; }
        public Int64 FacebookPostId { get; set; }
        public Int64 PersonFacebookId { get; set; }

        public AppReferral(Int64 appId, Int64 facebookPostId, Int64 personFacebookId)
        {
            this.AppId = appId;
            this.FacebookPostId = facebookPostId;
            this.PersonFacebookId = personFacebookId;
            DateTime = DateTime.Now;
        }
    }
}
