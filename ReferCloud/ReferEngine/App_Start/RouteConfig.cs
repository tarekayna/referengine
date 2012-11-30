<<<<<<< HEAD
﻿using System.Web.Mvc;
using System.Web.Routing;

namespace ReferEngine
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
                name: "Refer",
                url: "refer/{platform}/{action}/{id}",
                defaults: new { controller = "Refer", platform = "Win8", action = "Intro", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
=======
﻿using System.Web.Mvc;
using System.Web.Routing;

namespace ReferEngine
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
                name: "Refer",
                url: "refer/{platform}/{action}/{id}",
                defaults: new { controller = "Refer", platform = "Win8", action = "Intro", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
>>>>>>> Facebook Post and Commit to DB
}