using System.Web.Caching;
using ReferEngine.Common.Utilities;
using ReferEngine.Web.DataAccess;
using System;
using System.Web.Mvc;
using ReferEngine.Web.Models.Common;

namespace ReferEngine.Web.Controllers
{
    [OutputCache(Duration = 0)]
    public class InAppScriptController : BaseController
    {
        public InAppScriptController(IReferDataReader dataReader, IReferDataWriter dataWriter) : base(dataReader, dataWriter) { }

        public JavaScriptResult Get(string platform)
        {
            Verifier.IsNotNullOrEmpty(platform, "Platform is invalid");
            if (platform.Equals("Windows", StringComparison.OrdinalIgnoreCase))
            {
                bool useMinifiedJs = Util.CurrentServiceConfiguration != Util.ReferEngineServiceConfiguration.Local;

                const string cssRelativePath = "~/TypeScript/InAppScript/Windows/WindowsInAppStyle.min.css";
                string jsRelativePath = "~/TypeScript/InAppScript/Windows/WindowsInAppScript" + (useMinifiedJs ? ".min.js" : ".js");

                string js = System.IO.File.ReadAllText(Server.MapPath(jsRelativePath));
                string baseUrl = (new BaseUrlJavaScriptGlobalVariable()).Value;
                js = string.Format("ReferEngineClient.baseUrl=\"{0}\";{1}", baseUrl, js);

                string css = System.IO.File.ReadAllText(Server.MapPath(cssRelativePath));
                js = string.Format("ReferEngineClient.style=\"{0}\";{1}", css, js);

                return JavaScript(js);
            }

            throw new InvalidOperationException(string.Format("Invalid platform: {0}", platform));
        }
    }
}
