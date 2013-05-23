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
            ApplicationPopularityPerGenreQueue = new Queue(namespaceManager, messagingFactory, "AppPopularityByGenre", typeof(string));
            ApplicationPriceQueue = new Queue(namespaceManager, messagingFactory, "AppPrice", typeof(string));
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
                    accessInfo.Namespace = "ios-production";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "dQaG3CjJ5Mygco4ZQnQSpkZui3fC0vBSV8z4gr42edM=";
                    break;
                case Util.ReferEngineServiceConfiguration.TestCloud:
                    accessInfo.Namespace = "ios-test";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "l5tU3iRCXue6pd6FKzdu3r7WBFRPoRpOJata1ygHHx4=";
                    break;
                case Util.ReferEngineServiceConfiguration.Local:
                    accessInfo.Namespace = "ios-local";
                    accessInfo.Issuer = "owner";
                    accessInfo.Key = "l5tU3iRCXue6pd6FKzdu3r7WBFRPoRpOJata1ygHHx4=";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return accessInfo;
        }
    }
}