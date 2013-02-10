using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using System;
using System.Web.Mvc;
using System.Web.Optimization;

namespace ReferEngine.Web.Controllers
{
    [RemoteRequireHttps]
    public class InAppScriptController : BaseController
    {
        public InAppScriptController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public JavaScriptResult Get(string platform)
        {
            Verifier.IsNotNullOrEmpty(platform, "Platform is invalid");
            if (platform.Equals("Windows", StringComparison.OrdinalIgnoreCase))
            {
                string cssPath = Server.MapPath("~/Views/InAppScript/WindowsInAppStyle.css");
                string css = System.IO.File.ReadAllText(cssPath);
                var cssBundle = new Bundle("~/Views/InAppScript/WindowsInAppStyle");
                var cssBundleCollection = new BundleCollection { cssBundle };
                var cssContext = new BundleContext(HttpContext, cssBundleCollection, "~/Views/InAppScript/WindowsInAppStyle");
                var cssResponse = new BundleResponse(css, null);
                var cssMinify = new CssMinify();
                cssMinify.Process(cssContext, cssResponse);

                string path = Server.MapPath("~/Views/InAppScript/WindowsInAppScript.js");
                string js = System.IO.File.ReadAllText(path);
                js = string.Format("ReferEngine.StyleSheetContent = \"{0}\";{1}", cssResponse.Content, js);
                var bundle = new Bundle("~/Views/InAppScript/WindowsInAppScript");
                var bundleCollection = new BundleCollection {bundle};
                var context = new BundleContext(HttpContext, bundleCollection, "~/Views/InAppScript/WindowsInAppScript");
                var response = new BundleResponse(js, null);
                var minify = new JsMinify();
                minify.Process(context, response);
                return JavaScript(response.Content);
            }

            throw new InvalidOperationException(string.Format("Invalid platform: {0}", platform));
        }
    }
}
