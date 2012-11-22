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
        }
    }
}