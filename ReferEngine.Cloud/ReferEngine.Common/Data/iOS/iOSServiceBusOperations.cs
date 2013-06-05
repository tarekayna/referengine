using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ReferEngine.Common.Utilities;
using System;

namespace ReferEngine.Common.Data.iOS
{
    public static class iOSServiceBusOperations
    {
        public static Queue AppsQueue;
        public static Queue ArtistsQueue;
        public static Queue AppDetailsQueue;
        public static Queue MediaTypesQueue;
        public static Queue DeviceTypesQueue;
        public static Queue GenresQueue;
        public static Queue StorefrontsQueue;
        public static Queue ArtistTypesQueue;
        public static Queue ApplicationDeviceTypesQueue;
        public static Queue ArtistApplicationsQueue;
        public static Queue GenreApplicationsQueue;
        public static Queue ApplicationPopularityPerGenreQueue;
        public static Queue ApplicationPriceQueue;

        public static readonly IList<Queue> Queues = new List<Queue>();

        public static void Initialize()
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var namespaceManager = CreateNamespaceManager();
            MessagingFactorySettings messagingFactorySettings = new MessagingFactorySettings
            {
                TokenProvider = namespaceManager.Settings.TokenProvider,
            };
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address, messagingFactorySettings);

            ArtistsQueue = new Queue(namespaceManager, messagingFactory, "Artist", typeof(string));
            AppsQueue = new Queue(namespaceManager, messagingFactory, "App", typeof(string));
            AppDetailsQueue = new Queue(namespaceManager, messagingFactory, "AppDetail", typeof(string));
            MediaTypesQueue = new Queue(namespaceManager, messagingFactory, "MediaType", typeof(string));
            DeviceTypesQueue = new Queue(namespaceManager, messagingFactory, "DeviceType", typeof(string));
            GenresQueue = new Queue(namespaceManager, messagingFactory, "Genre", typeof(string));
            StorefrontsQueue = new Queue(namespaceManager, messagingFactory, "Storefront", typeof(string));
            ArtistTypesQueue = new Queue(namespaceManager, messagingFactory, "AristType", typeof(string));
            ApplicationDeviceTypesQueue = new Queue(namespaceManager, messagingFactory, "AppDeviceType", typeof(string));
            ArtistApplicationsQueue = new Queue(namespaceManager, messagingFactory, "ArtistApp", typeof(string));
            GenreApplicationsQueue = new Queue(namespaceManager, messagingFactory, "GenreApp", typeof(string));
            ApplicationPopularityPerGenreQueue = new Queue(namespaceManager, messagingFactory, "Popularity", typeof(string));
            ApplicationPriceQueue = new Queue(namespaceManager, messagingFactory, "AppPrice", typeof(string));

            Queues.Add(AppsQueue);
            Queues.Add(ArtistsQueue);
            Queues.Add(AppDetailsQueue);
            Queues.Add(MediaTypesQueue);
            Queues.Add(DeviceTypesQueue);
            Queues.Add(GenresQueue);
            Queues.Add(StorefrontsQueue);
            Queues.Add(ArtistTypesQueue);
            Queues.Add(ApplicationDeviceTypesQueue);
            Queues.Add(ArtistApplicationsQueue);
            Queues.Add(GenreApplicationsQueue);
            Queues.Add(ApplicationPopularityPerGenreQueue);
            Queues.Add(ApplicationPriceQueue);
        }

        private static NamespaceManager CreateNamespaceManager()
        {
            ServiceBusAccessInfo accessInfo = GetAccessInfo();
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", accessInfo.Namespace, String.Empty);
            TokenProvider tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(accessInfo.Issuer, accessInfo.Key);
            return new NamespaceManager(uri, tokenProvider);
        }

        private static ServiceBusAccessInfo GetAccessInfo()
        {
            ServiceBusAccessInfo accessInfo = new ServiceBusAccessInfo();

            switch (Util.CurrentServiceConfiguration)
            {
                case Util.ReferEngineServiceConfiguration.ProductionCloud:
                case Util.ReferEngineServiceConfiguration.Local:
                    accessInfo.Namespace = "ios-prodcloud";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "ib1jZGC6tO0NLoOPOgcrUAiTvMbr9QRF1wrxdh+jQzw=";
                    break;
                case Util.ReferEngineServiceConfiguration.TestCloud:
                    accessInfo.Namespace = "ios-test";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "+8VPpbs5AhHY/mEOw+kK+xqH0P9uZTkW9Gknfu9IP8k=";
                    break;
                //case Util.ReferEngineServiceConfiguration.Local:
                //    accessInfo.Namespace = "ios-local";
                //    accessInfo.Issuer = "owner";
                //    accessInfo.Key = "l5tU3iRCXue6pd6FKzdu3r7WBFRPoRpOJata1ygHHx4=";
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return accessInfo;
        }

        public static void RenewLocks(BrokeredMessage[] messages)
        {
            TimeSpan renewDelta = TimeSpan.FromMinutes(2);
            const int maxRetries = 3;
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    foreach (var brokeredMessage in messages.Where(brokeredMessage => DateTime.UtcNow > brokeredMessage.LockedUntilUtc.Subtract(renewDelta)))
                    {
                        brokeredMessage.RenewLock();
                    }
                    break;
                }
                catch (MessagingCommunicationException)
                {
                    // it's ok, try again
                }
            }
        }
    }
}