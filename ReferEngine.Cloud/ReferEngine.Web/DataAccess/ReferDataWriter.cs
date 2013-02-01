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

        public void AddAppAuthorization(AppAuthorization appAuthorization)
        {
            CacheOperations.AddAppAuthorization(appAuthorization, TimeSpan.FromMinutes(20));
            ServiceBusOperations.AddToQueue(appAuthorization);
        }

        public void AddAppReceipt(AppReceipt appReceipt)
        {
            ServiceBusOperations.AddToQueue(appReceipt);
        }

        public void AddAppScreenshot(AppScreenshot appScreenshot)
        {
            ServiceBusOperations.AddToQueue(appScreenshot);
        }

        public void AddAppRecommendation(AppRecommendation appRecommendation)
        {
            ServiceBusOperations.AddToQueue(appRecommendation);
        }

        public void AddPrivateBetaSignup(PrivateBetaSignup privateBetaSignup)
        {
            ServiceBusOperations.AddToQueue(privateBetaSignup);
        }

        public void AddUserRole(User user, string role)
        {
            DatabaseOperations.AddUserRole(user, role);
        }
    }
}