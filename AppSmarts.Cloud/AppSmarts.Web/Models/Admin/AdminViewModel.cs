using System.Collections.Generic;
using AppSmarts.Common.Data;
using AppSmarts.Common.Models;

namespace AppSmarts.Web.Models.Admin
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            UserMemberships = DataOperations.GetUserMemberships();
        }

        public IList<UserMembership> UserMemberships { get; set; }
    }
}