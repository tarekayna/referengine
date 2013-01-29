using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.Models.Admin
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            UserMemberships = DatabaseOperations.GetUserMemberships();
        }

        public IList<UserMembership> UserMemberships { get; set; }
    }
}