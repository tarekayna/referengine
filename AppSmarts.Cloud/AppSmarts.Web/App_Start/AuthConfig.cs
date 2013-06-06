using Microsoft.Web.WebPages.OAuth;

namespace AppSmarts.Web.App_Start
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

            //CloudinaryImage cloudinaryImage = new CloudinaryImage();
            //cloudinaryImage.Id = "FB_f_Logo__blue_1024_jz915q";
            //cloudinaryImage.Format = "png";

            //Dictionary<string, object> facebooksocialData = new Dictionary<string, object>
            //    {
            //        {"Icon", cloudinaryImage.GetLink("w_50")}
            //    };
            OAuthWebSecurity.RegisterFacebookClient(
                appId: "368842109866922",
                appSecret: "b673f45aa978225ae8c9e4817a726be7");
            //displayName: "Facebook",
            //extraData: facebooksocialData);

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}