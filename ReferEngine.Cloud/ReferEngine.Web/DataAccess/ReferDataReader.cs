using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public class ReferDataReader : IReferDataReader
    {
        public App GetApp(string packageFamilyName)
        {
            return DatabaseOperations.GetApp(packageFamilyName);
        }

        public App GetApp(long id)
        {
            return DatabaseOperations.GetApp(id);
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
            User user = CacheOperations.User.Get(id);
            if (user == null)
            {
                user = DatabaseOperations.GetUser(id);
                CacheOperations.User.Add(user);
            }
            return user;
        }

        public User GetUserFromConfirmationCode(string code)
        {
            return DatabaseOperations.GetUserFromConfirmationCode(code);
        }

        public IList<StoreAppInfo> FindStoreApps(string term, int count)
        {
            return DatabaseOperations.FindStoreApps(term, count);
        }
    }
}