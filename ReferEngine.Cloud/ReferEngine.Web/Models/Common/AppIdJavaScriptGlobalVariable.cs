using System;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public class AppIdJavaScriptGlobalVariable : JavaScriptGlobalVariable
    {
        public override string Name
        {
            get { return "appId"; }
        }

        public override string Value
        {
            get { return ViewProperties.CurrentApp == null ? null : ViewProperties.CurrentApp.Id.ToString(); }
        }
    }
}