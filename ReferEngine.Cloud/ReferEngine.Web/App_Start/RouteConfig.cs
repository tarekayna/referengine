using System.Web.Mvc;
using System.Web.Routing;

namespace ReferEngine.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FacebookMobile",
                url: "fb/m/{id}",
                defaults: new { controller = "FacebookMobile", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Facebook",
                url: "fb/{action}/{id}",
                defaults: new { controller = "Facebook", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Recommend",
                url: "recommend/{platform}/{action}/{id}",
                defaults: new { controller = "Recommend", platform = "Win8", action = "Intro", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}