using System;
using System.Collections.Generic;

namespace ReferEngine.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual List<App> Apps { get; set; }
    }
}
