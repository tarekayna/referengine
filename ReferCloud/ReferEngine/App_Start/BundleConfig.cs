using System.Web;
using System.Web.Optimization;

namespace ReferEngine
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/js/lib/jquery-{version}.js",
                        "~/js/lib/jquery-validate.js",
                        "~/js/lib/bootstrap.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                "~/css/bootstrap.css",
                "~/css/bootstrap-responsive.css",
                "~/css/font-awesome.css",
                "~/css/base.css",
                "~/css/blue.css"));

            bundles.Add(new StyleBundle("~/css/refer/win8").Include(
                "~/css/refer/win8/refer-win8.css"));

            bundles.Add(new ScriptBundle("~/js/refer/win8/intro").Include(
                "~/js/refer/win8/intro.js"));

            bundles.Add(new StyleBundle("~/css/refer/win8/intro").Include(
                "~/css/refer/win8/intro.css"));

            bundles.Add(new ScriptBundle("~/js/refer/win8/friends").Include(
                "~/js/refer/win8/friends.js"));

            bundles.Add(new StyleBundle("~/css/refer/win8/friends").Include(
                "~/css/refer/win8/friends.css"));
        }
    }
}