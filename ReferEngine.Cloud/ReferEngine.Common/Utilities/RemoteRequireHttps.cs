using System;
using System.Net;
using System.Web.Mvc;

namespace ReferEngine.Common.Utilities
{
    public class RemoteRequireHttps : RequireHttpsPermanent
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext != null && filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}