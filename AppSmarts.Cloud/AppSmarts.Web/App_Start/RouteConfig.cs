using System.Web.Mvc;
using System.Web.Routing;

namespace AppSmarts.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Apps",
                url: "apps/{platform}/{category}/{name}",
                defaults: new { controller = "AppStore", action = "Index", category = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            routes.MapRoute("Recommend", "recommend/{platform}/{action}/{id}", new { controller = "Recommend", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Home",
                url: "{action}",
                defaults: new { controller = "Home" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}