using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public class AppNameJavaScriptGlobalVariable : JavaScriptGlobalVariable
    {
        public override string Name
        {
            get { return "appName"; }
        }

        public override string Value 
        {
            get { return ViewProperties.CurrentApp == null ? null : ViewProperties.CurrentApp.Name; }
        }
    }
}