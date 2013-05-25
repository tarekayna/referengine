using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using ReferEngine.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferEngine.Common.Data
{
    public static class ServiceBusOperations
    {
        // When testing ServiceBusOperations locally, set this to false
        private static bool? _simulateWithoutConnecting;
        private static bool SimulateWithoutConnecting
        {
            get
            {
                if (!_simulateWithoutConnecting.HasValue)
                {
                    _simulateWithoutConnecting = Util.CurrentServiceConfiguration ==
                                                 Util.ReferEngineServiceConfiguration.Local;
                }

                return _simulateWithoutConnecting.Value;
            }
        }

        private static readonly IList<Queue> Queues = new List<Queue>();

        private static NamespaceManager CreateNamespaceManager()
        {
            ServiceBusAccessInfo accessInfo = GetAccessInfo();
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", accessInfo.Namespace, String.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(accessInfo.Issuer, accessInfo.Key);
            return new NamespaceManager(uri, tokenProvider);
        }

        public static void Initialize()
        {
            if (SimulateWithoutConnecting) return;

            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var namespaceManager = CreateNamespaceManager();
            MessagingFactorySettings messagingFactorySettings = new MessagingFactorySettings
                {
                    TokenProvider = namespaceManager.Settings.TokenProvider,
                };
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address,
                                                           messagingFactorySettings);

            // Add Queues by Priority
            Queues.Add(new Queue(namespaceManager, messagingFactory, "FacebookOperations", typeof(FacebookAccessSession)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppRecommendation", typeof(AppRecommendation)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppAuthorization", typeof(AppAuthorization)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "PrivateBetaSignup", typeof(PrivateBetaSignup)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppReceipt", typeof(AppReceipt)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "FacebookPageViewInfo", typeof(FacebookPageViewInfo)));
        }

        public static void AddToQueue(Object objectToQueue)
        {
            if (SimulateWithoutConnecting) return;

            Queue queue = Queues.First(q => q.SupportedType.IsEquivalentTo(objectToQueue.GetType()));
            queue.Enqueue(objectToQueue);
        }

        public static BrokeredMessage GetMessage()
        {
            if (SimulateWithoutConnecting) return null;
            BrokeredMessage message = null;
            foreach (Queue queue in Queues)
            {
                if (queue.IsEmpty()) continue;
                message = queue.Receive();
                if (message != null) break;
            }
            return message;
        }

        private static ServiceBusAccessInfo GetAccessInfo()
        {
            ServiceBusAccessInfo accessInfo = new ServiceBusAccessInfo();

            switch (Util.CurrentServiceConfiguration)
            {
                case Util.ReferEngineServiceConfiguration.ProductionCloud:
                    accessInfo.Namespace = "datawrite-production";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "HTrzqkC+7jWm35xqW/OM7wuFdfMUYfPQ8AZ60Jrjk20=";
                    break;
                case Util.ReferEngineServiceConfiguration.TestCloud:
                    accessInfo.Namespace = "datawrite-test";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "z2R6UQcSxeJFy29u/WE6sLy+XPiGpvjPBzFZEULgGjU=";
                    break;
                case Util.ReferEngineServiceConfiguration.Local:
                    accessInfo.Namespace = "datawrite-local";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "fGFQ+s3q85Wesyey4p1RD+TTnyY0dXBsoifBmOHuTvY=";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return accessInfo;
        }
    }
}