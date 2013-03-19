using System;
using System.Collections.Generic;
using System.Text;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;

namespace ReferEngine.Web.Models.Common
{
    public class ViewProperties
    {
        public bool UseJQueryValidate { get; set; }
        public bool UseJQueryUi { get; set; }
        public App CurrentApp { get; set; }
        public User CurrentUser { get; set; }
        public string ReturnUrl { get; set; }
        public string PageTitle { get; set; }

        public string SuccessMessage { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }
        public string InfoMessage { get; set; }

        public bool HasLocalPassword { get; set; }
        public bool ShowRemoveButton { get; set; }
        public string ProviderDisplayName { get; set; }

        public string ReferEngineAuthToken { get; set; }

        public string GetJavaScriptGlobalVariables()
        {
            string format = "ReferEngineGlobals.{0} = \"{1}\";";
            StringBuilder result = new StringBuilder();

            if (CurrentApp != null)
            {
                result.AppendLine(string.Format(format, "appId", CurrentApp.Id));
                result.AppendLine(string.Format(format, "appName", CurrentApp.Name));
            }

            string baseUrl;
            switch (Util.CurrentServiceConfiguration)
            {
                case Util.ReferEngineServiceConfiguration.ProductionCloud:
                    baseUrl = "https://www.referengine.com";
                    break;
                case Util.ReferEngineServiceConfiguration.TestCloud:
                    baseUrl = "https://www.referengine-test.com";
                    break;
                case Util.ReferEngineServiceConfiguration.Local:
                    baseUrl = "http://127.0.0.1:81";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            result.AppendLine(string.Format(format, "baseUrl", baseUrl));

            if (!string.IsNullOrEmpty(ReferEngineAuthToken))
            {
                result.AppendLine(string.Format(format, "referEngineAuthToken", ReferEngineAuthToken));
            }
            return result.ToString();
        }
    }
}