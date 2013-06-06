using System.Net;
using System.Web.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AppSmarts.Common.Utilities
{
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Unsealed because type contains virtual extensibility points.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireHttpsPermanentRemoteAttribute : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                HandleNonHttpsRequest(filterContext);
            }
        }

        protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            // Only redirect for GET requests, otherwise the browser 
            // might not propagate the verb and request body correctly.

            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Redirect to HTTPS version of page
            string url = "https://" + filterContext.HttpContext.Request.Url.Host +
                         filterContext.HttpContext.Request.RawUrl;

            // Redirect as a 301
            filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.MovedPermanently;
            filterContext.HttpContext.Response.RedirectLocation = url;
        }
    }
}