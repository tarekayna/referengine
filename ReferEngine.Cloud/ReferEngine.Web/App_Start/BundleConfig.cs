using System.Collections.Generic;
using System.Web.Optimization;

namespace ReferEngine.Web.App_Start
{
    public class BundleConfig
    {
        private class BundleInfo
        {
            public string VirtualPath { get; private set; }
            public IList<string> Files { get; private set; }

            public BundleInfo(string virtualPath, params string[] files)
            {
                VirtualPath = virtualPath;
                Files = new List<string>();
                foreach (string file in files)
                {
                    Files.Add(file);
                }
            }
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Script Bundles
            List<BundleInfo> scriptBundles = new List<BundleInfo>
                {
                    new BundleInfo("~/bundles/libs/base",
                                   "~/typescript/lib/jquery-1.8.3.js",
                                   "~/js/lib/jquery-validate.js",
                                   "~/typescript/lib/bootstrap.js",
                                   "~/typescript/lib/bootstrap-notify.js"),

                    new BundleInfo("~/bundles/js/defaultLayout",
                                   "~/typescript/lib/jquery-1.8.3.js",
                                   "~/js/lib/jquery-validate.js",
                                   "~/typescript/lib/bootstrap.js",
                                   "~/typescript/lib/bootstrap-notify.js",
                                   "~/typescript/lib/jquery.easing.1.3.js",
                                   "~/typescript/lib/google-code-prettify/prettify.js",
                                   "~/typescript/lib/modernizr.js",
                                   "~/typescript/lib/jquery.elastislide.js",
                                   "~/typescript/lib/jquery.flexslider.js",
                                   "~/typescript/lib/jquery.tweet.js",
                                   "~/typescript/lib/application.js",
                                   "~/typescript/lib/jquery.prettyPhoto.js",
                                   "~/typescript/lib/portfolio/jquery.quicksand.js",
                                   "~/typescript/lib/portfolio/setting.js",
                                   "~/typescript/lib/hover/jquery-hover-effect.js",
                                   "~/typescript/lib/hover/setting.js",
                                   "~/typescript/lib/custom.js"),
                                   
                    new BundleInfo("~/bundles/libs/jquery-ui",
                                   "~/js/lib/jquery-ui-1.10.0.custom.js"),
    
                    new BundleInfo("~/bundles/js/jquery.fitvids",
                                   "~/js/lib/jquery.fitvids.js"),

                    new BundleInfo("~/js/shared/privateBeta",
                                   "~/js/shared/privateBeta.js"),

                    new BundleInfo("~/js/contact/index",
                                   "~/js/contact/index.js"),
                                   
                    new BundleInfo("~/js/home",
                                   "~/js/home.js"),
                                   
                    new BundleInfo("~/js/pricing",
                                   "~/js/pricing.js"),

                    new BundleInfo("~/js/shared/layout-not-auth",
                                   "~/js/shared/layout-not-auth.js"),

                    new BundleInfo("~/js/app/edit",
                                   "~/js/app/edit.js"),
                };
            #endregion Script Bundles

            #region Style Bundles
            List<BundleInfo> styleBundles = new List<BundleInfo>
                {
                    new BundleInfo("~/bundles/css/bootstrap",
                                   "~/css/bootstrap.css",
                                   "~/css/bootstrap-notify.css",
                                   "~/css/font-awesome.css",
                                   "~/css/m-buttons.css",
                                   "~/css/base.css",
                                   "~/css/blue.css"),
                                   
                    new BundleInfo("~/bundles/css/defaultLayout",
                                   "~/less/shared/bootstrap/bootstrap.css",
                                   "~/less/shared/bootstrap/responsive.css",
                                   "~/css/AppStore/docs.css",
                                   "~/css/AppStore/style.css",
                                   "~/css/AppStore/success.css",
                                   "~/css/AppStore/prettyPhoto.css",
                                   "~/css/AppStore/prettify.css",
                                   "~/css/AppStore/boxtile.css",
                                   "~/css/bootstrap-notify.css",
                                   "~/less/shared/defaultLayout.css"),

                    new BundleInfo("~/bundles/home",
                                   "~/less/home/index.css"),

                    new BundleInfo("~/bundles/less/recommend/win8/intro",
                                   "~/less/recommend/win8/intro.css"),

                    new BundleInfo("~/bundles/less/recommend/win8/recommend",
                                   "~/less/recommend/win8/recommend.css"),

                    new BundleInfo("~/css/lib/jquery.aceditable",
                                   "~/css/lib/jquery.aceditable.css"),

                    new BundleInfo("~/css/app/edit",
                                   "~/css/app/edit.css"),

                    new BundleInfo("~/less/app/dashboard",
                                   "~/css/daterangepicker.css",
                                   "~/less/app/dashboard.css"),

                    new BundleInfo("~/less/app/settings",
                                   "~/less/app/settings.css"),

                    new BundleInfo("~/less/appstore/windows/app",
                                   "~/less/appstore/windows/windowsApp.css"),

                    new BundleInfo("~/less/app/new",
                                   "~/css/lib/jquery-ui.css",
                                   "~/less/app/new.css"),

                    new BundleInfo("~/bundles/less/fb/app",
                                   "~/less/fb/app.css")
                };
            #endregion Style Bundles

            foreach (BundleInfo bundleInfo in scriptBundles)
            {
                ScriptBundle scriptBundle = new ScriptBundle(bundleInfo.VirtualPath);
                foreach (string file in bundleInfo.Files)
                {
                    scriptBundle.Include(file);
                }
                bundles.Add(scriptBundle);
            }

            foreach (BundleInfo bundleInfo in styleBundles)
            {
                StyleBundle styleBundle = new StyleBundle(bundleInfo.VirtualPath);
                foreach (string file in bundleInfo.Files)
                {
                    styleBundle.Include(file);
                }
                bundles.Add(styleBundle);
            }
        }
    }
}