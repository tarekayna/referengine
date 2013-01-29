using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferEngine.Common.Models
{
    public class UserMembership
    {
        public UserMembership()
        {
            
        }

        public UserMembership(User user, Membership membership, string role)
        {
            User = user;
            Membership = membership;
            Role = role;
        }

        public User User { get; set; }
        public Membership Membership { get; set; }
        public string Role { get; set; }
    }
}
