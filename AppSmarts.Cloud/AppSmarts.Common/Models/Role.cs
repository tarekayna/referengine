using AppSmarts.Common.Utilities;

namespace AppSmarts.Common.Models
{
    public enum Roles
    {
        [StringValue("Admin")]
        Admin = 1,

        [StringValue("Dev")]
        Dev,

        [StringValue("User")]
        User
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
