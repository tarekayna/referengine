using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ReferEngineWeb.DataAccess
{
    public static class ServiceBusOperations
    {
        public static QueueClient UserAndFriendsQueueClient;
        public static QueueClient AppRecommendationQueueClient;
        public const string Namespace = "referenginedatabus";
        public const string IssuerName = "owner";
        public const string IssuerKey = "zvWWDVXb0SmLvs8mdWjLnKeszs1ETvWVMZ9SPGCCRCk=";

        public const string UserAndFriendsQueueName = "UserAndFriendsQueue";
        public const string AppRecommendationQueueName = "AppRecommendationQueue";

        private static NamespaceManager CreateNamespaceManager()
        {
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", Namespace, String.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(IssuerName, IssuerKey);
            return new NamespaceManager(uri, tokenProvider);
        }

        public static void Initialize()
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var namespaceManager = CreateNamespaceManager();
            if (!namespaceManager.QueueExists(UserAndFriendsQueueName))
            {
                namespaceManager.CreateQueue(UserAndFriendsQueueName);
            }
            if (!namespaceManager.QueueExists(AppRecommendationQueueName))
            {
                namespaceManager.CreateQueue(AppRecommendationQueueName);
            }

            // Get a client to the queue
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address, namespaceManager.Settings.TokenProvider);
            UserAndFriendsQueueClient = messagingFactory.CreateQueueClient(UserAndFriendsQueueName);
            AppRecommendationQueueClient = messagingFactory.CreateQueueClient(AppRecommendationQueueName);
        }
    }
}