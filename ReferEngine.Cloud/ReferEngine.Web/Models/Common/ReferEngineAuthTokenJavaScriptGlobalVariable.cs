using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public class ReferEngineAuthTokenJavaScriptGlobalVariable : JavaScriptGlobalVariable
    {
        public override string Name
        {
            get { return "referEngineAuthToken"; }
        }
    }
}