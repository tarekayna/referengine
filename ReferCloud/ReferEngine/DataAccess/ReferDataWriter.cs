using System;
using Microsoft.ServiceBus.Messaging;
using ReferLib;

namespace ReferEngineWeb.DataAccess
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

        public void AddPersonAndFriends(CurrentUser currentUser)
        {
            BrokeredMessage message = new BrokeredMessage(currentUser);
            ServiceBusOperations.UserAndFriendsQueueClient.Send(message);
        }

        public void AddAppRecommendation(AppRecommendation appRecommendation)
        {
            BrokeredMessage message = new BrokeredMessage(appRecommendation);
            ServiceBusOperations.AppRecommendationQueueClient.Send(message);
        }
    }
}