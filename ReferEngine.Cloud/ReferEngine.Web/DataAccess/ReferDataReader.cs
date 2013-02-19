using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;
using System.Linq;
using ReferEngine.Common.ViewModels;

namespace ReferEngine.Web.DataAccess
{
    public class ReferDataReader : IReferDataReader
    {
        public App GetApp(string packageFamilyName)
        {
            App app = CacheOperations.GetApp(packageFamilyName);
            if (app == null)
            {
                app = DatabaseOperations.GetApp(packageFamilyName);
                CacheOperations.AddApp(packageFamilyName, app);
            }
            return app;
        }

        public App GetApp(long id)
        {
            App app = CacheOperations.GetApp(id);
            if (app == null)
            {
                app = DatabaseOperations.GetApp(id);
                CacheOperations.AddApp(id, app);
            }
            return app;
        }

        public AppAuthorization GetAppAuthorization(string token, string userHostAddress)
        {
            return CacheOperations.GetAppAuthorization(token);
        }

        public FacebookOperations GetFacebookOperations(string referEngineAuthToken)
        {
            return CacheOperations.GetFacebookOperations(referEngineAuthToken);
        }

        public AppScreenshot GetAppScreenshot(long appId, string description)
        {
            return CacheOperations.GetAppScreenshot(appId, description);
        }

        public AppReceipt GetAppReceipt(string id)
        {
            return DatabaseOperations.GetAppReceipt(id);
        }

        public AppRecommendation GetAppRecommendation(long appId, long personFacebookId)
        {
            return DatabaseOperations.GetAppRecommdation(appId, personFacebookId);
        }

        public User GetUser(int id)
        {
            User user = CacheOperations.GetUser(id);
            if (user == null)
            {
                user = DatabaseOperations.GetUser(id);
                CacheOperations.AddUser(user);
            }
            return user;
        }

        public User GetUserFromConfirmationCode(string code)
        {
            return DatabaseOperations.GetUserFromConfirmationCode(code);
        }

        public AppDashboardViewModel GetAppDashboardViewModel(App app)
        {
            return DatabaseOperations.GetAppDashboardViewModel(app);
        }

        public IList<StoreAppInfo> FindStoreApps(string term, int count)
        {
            return DatabaseOperations.FindStoreApps(term, count);
        }
    }
}