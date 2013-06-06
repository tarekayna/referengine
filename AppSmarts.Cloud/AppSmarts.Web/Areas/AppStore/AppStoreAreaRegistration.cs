using System.Web.Mvc;

namespace AppSmarts.Web.Areas.AppStore
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
                "app-store/{controller}/a/{action}"
            );

            context.MapRoute(
                "AppStore_SubCategory_default",
                "app-store/{controller}/s/{parentCategory}/{category}/{name}",
                new { action = "ParentCategory", name = UrlParameter.Optional }
            );

            context.MapRoute(
                "AppStore_default",
                "app-store/{controller}/{category}/{name}",
                new { action = "Category", category = UrlParameter.Optional, name = UrlParameter.Optional }
            );
        }
    }
}
