using ReferEngine.Common;
using ReferEngine.Common.Models;
using System.Linq;

namespace ReferEngine.Web.DataAccess
{
    public class ReferDataReader : IReferDataReader
    {
        public App GetApp(string packageFamilyName)
        {
            App app = CacheOperations.GetApp(packageFamilyName);
            if (app == null)
            {
                using (var db = new ReferDb())
                {
                    app = db.Apps.First(a => a.PackageFamilyName == packageFamilyName);
                    CacheOperations.AddApp(packageFamilyName, app);
                }
            }
            return app;
        }

        public App GetApp(long id)
        {
            App app = CacheOperations.GetApp(id);
            if (app == null)
            {
                using (var db = new ReferDb())
                {
                    app = db.Apps.First(a => a.Id == id);
                    CacheOperations.AddApp(id, app);
                }
            }
            return app;
        }

        public AppAuthorization GetAppAuthorization(string token, string userHostAddress)
        {
            return CacheOperations.GetAppAuthorization(token, userHostAddress);
        }

        public FacebookOperations GetFacebookOperations(string referEngineAuthToken)
        {
            return CacheOperations.GetFacebookOperations(referEngineAuthToken);
        }

        public AppScreenshot GetAppScreenshot(long appId, string description)
        {
            return CacheOperations.GetAppScreenshot(appId, description);
        }
    }
}