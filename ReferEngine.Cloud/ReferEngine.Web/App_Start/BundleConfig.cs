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
                    new BundleInfo("~/bundles/libs",
                                   "~/js/lib/jquery-{version}.js",
                                   "~/js/lib/jquery-validate.js",
                                   "~/js/lib/bootstrap.js"),
    
                    new BundleInfo("~/bundles/js/jquery.fitvids",
                                   "~/js/lib/jquery.fitvids.js"),

                    new BundleInfo("~/js/home/index",
                                   "~/js/home/index.js"),

                    new BundleInfo("~/js/contact/index",
                                   "~/js/contact/index.js"),

                    new BundleInfo("~/bundles/js/recommend/win8/intro",
                                   "~/js/recommend/win8/common.js",
                                   "~/js/recommend/win8/intro.js"),
                                   
                    new BundleInfo("~/js/recommend/win8/recommend",
                                   "~/js/lib/knockout.js",
                                   "~/js/recommend/win8/common.js",
                                   "~/js/recommend/win8/recommend.js"),

                    new BundleInfo("~/js/lib/jquery.aceditable",
                                   "~/js/lib/jquery.aceditable.js"),

                    new BundleInfo("~/js/app/edit",
                                   "~/js/app/edit.js")
                };
            #endregion Script Bundles

            #region Style Bundles
            List<BundleInfo> styleBundles = new List<BundleInfo>
                {
                    new BundleInfo("~/bundles/css/bootstrap",
                                   "~/css/bootstrap.css",
                                   "~/css/bootstrap-responsive.css",
                                   "~/css/font-awesome.css",
                                   "~/css/m-buttons.css",
                                   "~/css/base.css",
                                   "~/css/blue.css"),

                    new BundleInfo("~/bundles/less/web",
                                   "~/less/web/main.css"),

                    new BundleInfo("~/bundles/less/recommend/win8/intro",
                                   "~/less/recommend/win8/intro.css"),

                    new BundleInfo("~/bundles/less/recommend/win8/recommend",
                                   "~/less/recommend/win8/recommend.css"),

                    new BundleInfo("~/css/lib/jquery.aceditable",
                                   "~/css/lib/jquery.aceditable.css"),

                    new BundleInfo("~/css/app/edit",
                                   "~/css/app/edit.css"),

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