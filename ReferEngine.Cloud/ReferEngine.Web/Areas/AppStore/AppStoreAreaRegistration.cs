using System.Web.Mvc;

namespace ReferEngine.Web.Areas.AppStore
{
    public class AppStoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AppStore";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AppStore_action",
                "AppStore/{controller}/{action}"
            );

            context.MapRoute(
                "AppStore_default",
                "AppStore/{controller}/{category}/{name}",
                new { action = "Index", category = UrlParameter.Optional, name = UrlParameter.Optional }
            );
        }
    }
}
