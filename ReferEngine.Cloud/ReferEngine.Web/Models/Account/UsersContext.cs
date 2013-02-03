using System.Data.Entity;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Account
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base(Util.DatabaseConnectionStringName)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
