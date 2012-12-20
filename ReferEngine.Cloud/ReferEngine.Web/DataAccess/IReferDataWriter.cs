using System;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public interface IReferDataWriter
    {
        void AddFacebookOperations(string referEngineAuthToken, FacebookOperations facebookOperations);
        void AddPersonAndFriends(CurrentUser currentUser);
        void AddAppRecommendation(AppRecommendation appRecommendation);
        void AddAppAuthorization(AppAuthorization appAuthorization, TimeSpan expiresIn);
        void AddAppScreenshot(AppScreenshot appScreenshot);
        void AddAppReceipt(AppReceipt appReceipt);
    }
}
