using System;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public class ReferDataWriter : IReferDataWriter
    {
        public void AddFacebookOperations(string referEngineAuthToken, FacebookOperations facebookOperations)
        {
            CacheOperations.AddFacebookOperations(referEngineAuthToken, facebookOperations);
        }

        public void AddAppAuthorization(AppAuthorization appAuthorization, TimeSpan expiresIn)
        {
            CacheOperations.AddAppAuthorization(appAuthorization, expiresIn);
        }

        public void AddAppReceipt(AppReceipt appReceipt)
        {
            ServiceBusOperations.AddToQueue(appReceipt);
        }

        public void AddAppScreenshot(AppScreenshot appScreenshot)
        {
            ServiceBusOperations.AddToQueue(appScreenshot);
        }

        public void AddPersonAndFriends(CurrentUser currentUser)
        {
            ServiceBusOperations.AddToQueue(currentUser);
        }

        public void AddAppRecommendation(AppRecommendation appRecommendation)
        {
            ServiceBusOperations.AddToQueue(appRecommendation);
        }
    }
}