using System;
using System.Collections.Generic;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Models;
using System.Linq;

namespace ReferEngine.Common.Data
{
    public static class ServiceBusOperations
    {
        private const string Namespace = "referenginedatabus";
        private const string IssuerName = "owner";
        private const string IssuerKey = "zvWWDVXb0SmLvs8mdWjLnKeszs1ETvWVMZ9SPGCCRCk=";
        private static readonly IList<Queue> Queues = new List<Queue>();

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
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address,
                                                           namespaceManager.Settings.TokenProvider);

            Queues.Add(new Queue(namespaceManager, messagingFactory, "CurrentUser", typeof(CurrentUser)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppRecommendation", typeof(AppRecommendation)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppReceipt", typeof(AppReceipt)));
            Queues.Add(new Queue(namespaceManager, messagingFactory, "AppScreenshot", typeof(AppScreenshot)));
        }

        public static void AddToQueue(Object objectToQueue)
        {
            Queue queue = Queues.First(q => q.SupportedType.IsEquivalentTo(objectToQueue.GetType()));
            queue.Enqueue(objectToQueue);
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