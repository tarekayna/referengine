using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public interface IReferDataReader
    {
        App GetApp(long id);
        App GetApp(string packageFamilyName);
        AppAuthorization GetAppAuthorization(string token, string userHostAddress);
        FacebookOperations GetFacebookOperations(string referEngineAuthToken);
        AppScreenshot GetAppScreenshot(long id, string description);
        AppReceipt GetAppReceipt(string id);
        AppRecommendation GetAppRecommendation(long appId, long personFacebookId);
    }
}
