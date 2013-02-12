using System.Collections.Generic;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.Models.Common
{
    public static class ViewProperties
    {
        static ViewProperties()
        {
            JavaScriptGlobalVariables = new List<IJavaScriptGlobalVariable>
                                            {
                                                new BaseUrlJavaScriptGlobalVariable(),
                                                new AppNameJavaScriptGlobalVariable(),
                                                new AppIdJavaScriptGlobalVariable(),
                                                new ReferEngineAuthTokenJavaScriptGlobalVariable()
                                            };
        }

        public static bool UseJQueryValidate { get; set; }
        public static bool UseJQueryUi { get; set; }
        public static App CurrentApp { get; set; }
        public static User CurrentUser { get; set; }
        public static string ReturnUrl { get; set; }

        public static string SuccessMessage { get; set; }
        public static string StatusMessage { get; set; }
        public static string ErrorMessage { get; set; }
        public static string WarningMessage { get; set; }
        public static string InfoMessage { get; set; }

        public static bool HasLocalPassword { get; set; }
        public static bool ShowRemoveButton { get; set; }
        public static string ProviderDisplayName { get; set; }

        public static IList<IJavaScriptGlobalVariable> JavaScriptGlobalVariables { get; private set; }
    }
}