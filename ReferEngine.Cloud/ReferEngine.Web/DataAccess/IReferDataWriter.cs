using System;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public interface IReferDataWriter
    {
        void AddFacebookOperations(string referEngineAuthToken, FacebookOperations facebookOperations);
        void AddAppRecommendation(AppRecommendation appRecommendation);
        void AddAppAuthorization(AppAuthorization appAuthorization);
        void AddAppScreenshot(AppScreenshot appScreenshot);
        void AddAppReceipt(AppReceipt appReceipt);
        void AddPrivateBetaSignup(PrivateBetaSignup privateBetaSignup);
    }
}
