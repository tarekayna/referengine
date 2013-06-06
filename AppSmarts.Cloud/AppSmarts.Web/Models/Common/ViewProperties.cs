using System;
using System.Text;
using AppSmarts.Common.Data;
using AppSmarts.Common.Models;
using AppSmarts.Common.Utilities;

namespace AppSmarts.Web.Models.Common
{
    public class ViewProperties
    {
        public User CurrentUser { get; set; }
        public FacebookAccessSession FacebookAccessSession { get; set; }
        public bool HasCurrentUser { get { return CurrentUser != null; } }
        public bool HasFacebookAccessSession { get { return FacebookAccessSession != null; } }

        public App CurrentApp { get; set; }

        public string ReturnUrl { get; set; }
        public string PageTitle { get; set; }
        public string ActiveMenuItem { get; set; }

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
            StringBuilder result = new StringBuilder();
            result.AppendLine("window.re = {};");
            const string format = "re.{0} = \"{1}\";";

            if (CurrentApp != null)
            {
                result.AppendLine(string.Format(format, "appId", CurrentApp.Id));
                result.AppendLine(string.Format(format, "appName", CurrentApp.Name));
            }

            string baseUrl;
            switch (Util.CurrentServiceConfiguration)
            {
                case Util.AppSmartsServiceConfiguration.ProductionCloud:
                    baseUrl = "https://www.referengine.com";
                    break;
                case Util.AppSmartsServiceConfiguration.TestCloud:
                    baseUrl = "https://www.referengine-test.com";
                    break;
                case Util.AppSmartsServiceConfiguration.Local:
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