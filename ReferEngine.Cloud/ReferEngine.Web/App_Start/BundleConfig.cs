using System.Web.Optimization;

namespace ReferEngine.Web.App_Start
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
                "~/css/m-buttons.css",
                "~/css/base.css",
                "~/css/blue.css"));

            bundles.Add(new StyleBundle("~/css/refer/win8").Include(
                "~/css/refer/win8/refer-win8.css"));

            bundles.Add(new ScriptBundle("~/js/refer/win8/intro").Include(
                "~/js/refer/win8/common.js",
                "~/js/refer/win8/intro.js"));

            bundles.Add(new StyleBundle("~/css/refer/win8/intro").Include(
                "~/css/refer/win8/intro.css"));

            bundles.Add(new ScriptBundle("~/js/refer/win8/recommend").Include(
                "~/js/lib/knockout.js",
                "~/js/refer/win8/common.js",
                "~/js/refer/win8/recommend.js"));

            bundles.Add(new StyleBundle("~/css/refer/win8/recommend").Include(
                "~/css/refer/win8/recommend.css"));

            bundles.Add(new ScriptBundle("~/js/lib/jquery.aceditable").Include(
                "~/js/lib/jquery.aceditable.js"));

            bundles.Add(new StyleBundle("~/css/lib/jquery.aceditable").Include(
                "~/css/lib/jquery.aceditable.css"));

            bundles.Add(new StyleBundle("~/css/app/edit").Include(
                "~/css/app/edit.css"));

            bundles.Add(new ScriptBundle("~/js/app/edit").Include(
                "~/js/app/edit.js"));
        }
    }
}