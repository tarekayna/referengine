using Microsoft.Web.WebPages.OAuth;

namespace ReferEngine.Web.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "368842109866922",
                appSecret: "b673f45aa978225ae8c9e4817a726be7");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}