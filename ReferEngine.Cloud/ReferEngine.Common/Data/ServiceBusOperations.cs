using System;
using System.Collections.Generic;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using System.Linq;
using ReferEngine.Common.Properties;

namespace ReferEngine.Common.Data
{
    public static class ServiceBusOperations
    {
        private static readonly IList<Queue> Queues = new List<Queue>();

        private static NamespaceManager CreateNamespaceManager()
        {
            string dataWriteNamespace = Settings.Default.DataWriteBusNamespace;
            string issuerName = Settings.Default.DataWriteBusIssuerName;
            string issuerKey = Settings.Default.DataWriteBusIssuerKey;
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", dataWriteNamespace, String.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);
            return new NamespaceManager(uri, tokenProvider);
        }

        public static void Initialize(bool isLocal)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var namespaceManager = CreateNamespaceManager();
            MessagingFactorySettings messagingFactorySettings = new MessagingFactorySettings
                {
                    TokenProvider = namespaceManager.Settings.TokenProvider,
                    //OperationTimeout = DefaultOperationTimeout
                };
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address,
                                                           messagingFactorySettings);

            string suffix = isLocal ? "_Local" : "_Cloud";

            // Add Queues by Priority
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppAuthorization" + suffix, typeof(AppAuthorization)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "FacebookOperations" + suffix, typeof(FacebookOperations)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppRecommendation" + suffix, typeof(AppRecommendation)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppScreenshot" + suffix, typeof(AppScreenshot)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "PrivateBetaSignup" + suffix, typeof(PrivateBetaSignup)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppReceipt" + suffix, typeof(AppReceipt)));
        }

        public static void AddToQueue(Object objectToQueue)
        {
            Queue queue = Queues.First(q => q.SupportedType.IsEquivalentTo(objectToQueue.GetType()));
            queue.Enqueue(objectToQueue);
        }

        public static BrokeredMessage GetMessage()
        {
            BrokeredMessage message = null;
            foreach (Queue queue in Queues)
            {
                message = queue.Receive();
                if (message != null)
                {
                    break;
                }
            }
            return message;
        }

        private class Queue
        {
            private string Name { get; set; }
            private NamespaceManager NamespaceManager { get; set; }
            private MessagingFactory MessagingFactory { get; set; }
            public Type SupportedType { get; private set; }
            private QueueClient Client { get; set; }

            public Queue(NamespaceManager namespaceManager, MessagingFactory messagingFactory, string queueName, Type supportedType)
            {
                NamespaceManager = namespaceManager;
                MessagingFactory = messagingFactory;
                Name = queueName;
                SupportedType = supportedType;
                CreateIfNeeded();
                Client = messagingFactory.CreateQueueClient(Name);
            }

            public void Enqueue(Object obj)
            {
                if (obj.GetType() == SupportedType)
                {
                    Client.Send(new BrokeredMessage(obj));
                }
            }

            public BrokeredMessage Receive()
            {
                TimeSpan timeSpan = TimeSpan.FromMinutes(0);
                BrokeredMessage message = Client.Receive(timeSpan);

                if (message != null)
                {
                    message.ContentType = SupportedType.ToString();
                }
                return message;
            }

            private void CreateIfNeeded()
            {
                if (NamespaceManager.QueueExists(Name))
                {
                    return;
                }
                try
                {
                    NamespaceManager.CreateQueue(Name);
                }
                catch (MessagingEntityAlreadyExistsException)
                {
                    // already exists, good
                }
            }
        }
    }
}