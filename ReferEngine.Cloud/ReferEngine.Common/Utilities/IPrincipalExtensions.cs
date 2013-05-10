using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Utilities
{
    public static class PrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal principal, Roles role)
        {
            return principal.IsInRole(role.GetStringValue());
        }
    }
}
