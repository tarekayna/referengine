using System.Security.Principal;
using AppSmarts.Common.Models;

namespace AppSmarts.Common.Utilities
{
    public static class PrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal principal, Roles role)
        {
            return principal.IsInRole(role.GetStringValue());
        }
    }
}
