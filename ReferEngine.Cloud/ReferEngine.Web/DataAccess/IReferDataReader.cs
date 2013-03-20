using System.Collections.Generic;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public interface IReferDataReader
    {
        App GetApp(long id);
        AppAuthorization GetAppAuthorization(string token, string userHostAddress);
        FacebookOperations GetFacebookOperations(string referEngineAuthToken);
        AppScreenshot GetAppScreenshot(long id, string description);
        AppReceipt GetAppReceipt(string id);
        AppRecommendation GetAppRecommendation(long appId, long personFacebookId);
        User GetUser(int id);
        User GetUserFromConfirmationCode(string code);
        IList<StoreAppInfo> FindStoreApps(string term, int count);
    }
}
