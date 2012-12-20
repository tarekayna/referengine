using System.Web.Http;

namespace ReferEngineWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{platform}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
