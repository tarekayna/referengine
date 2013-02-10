using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public class BaseUrlJavaScriptGlobalVariable : IJavaScriptGlobalVariable
    {
        public string Name { get { return "BaseUrl"; } }
        public string Value
        {
            get
            {
                switch (Util.CurrentServiceConfiguration)
                {
                    case Util.ReferEngineServiceConfiguration.ProductionCloud:
                        return "https://www.referengine.com/";
                    case Util.ReferEngineServiceConfiguration.TestCloud:
                        return "https://www.referengine-test.com/";
                    case Util.ReferEngineServiceConfiguration.Local:
                        return "http://127.0.0.1:81/";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}