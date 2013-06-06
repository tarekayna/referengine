using System.Collections.Generic;
using AppSmarts.Common.Models.iOS;
using AppSmarts.Common.ViewModels.AppStore.iOS;
using Microsoft.ApplicationServer.Caching;
using System.Globalization;

namespace AppSmarts.Common.Data.iOS
{
    public static class iOSCacheOperations
    {
        internal class AppCacheEntity : CachedEntity<iOSApp>
        {
            private const string KeyFormat = "ios-app-id-{0}";

            public AppCacheEntity(DataCache cache)
                : base(cache, KeyFormat)
            {
            }

            public void Remove(iOSApp app)
            {
                base.Remove(app.Id.ToString(CultureInfo.InvariantCulture));
            }

            public iOSApp Get(int id)
            {
                return base.Get(id.ToString(CultureInfo.InvariantCulture));
            }

            public void Add(iOSApp newEntry)
            {
                base.Add(newEntry, DefaultTimeout, newEntry.Id.ToString(CultureInfo.InvariantCulture));
            }
        }
        private static AppCacheEntity _app;
        internal static AppCacheEntity App
        {
            get { return _app ?? (_app = new AppCacheEntity(CacheOperations.Cache)); }
        }

        internal class iOSAppViewModelByNameCacheEntity : CachedEntity<iOSAppViewModel>
        {
            private const string KeyFormat = "ios-app-viewmodel-name-{0}";

            public iOSAppViewModelByNameCacheEntity(DataCache cache)
                : base(cache, KeyFormat)
            {
            }

            public void Remove(iOSAppViewModel entry)
            {
                base.Remove(entry.App.Title);
            }

            public iOSAppViewModel Get(string name)
            {
                return base.Get(name);
            }

            public void Add(iOSAppViewModel newEntry)
            {
                base.Add(newEntry, DefaultTimeout, newEntry.App.Title);
            }
        }
        private static iOSAppViewModelByNameCacheEntity _appViewModelByName;
        internal static iOSAppViewModelByNameCacheEntity AppViewModelByName
        {
            get { return _appViewModelByName ?? (_appViewModelByName = new iOSAppViewModelByNameCacheEntity(CacheOperations.Cache)); }
        }

        internal class AppStoreGenresCacheEntity : CachedEntity<IList<iOSGenre>>
        {
            private const string KeyFormat = "ios-appstore-genres";

            public AppStoreGenresCacheEntity(DataCache cache)
                : base(cache, KeyFormat)
            {
            }

            public IList<iOSGenre> Get()
            {
                return base.Get();
            }

            public void Add(IList<iOSGenre> entry)
            {
                base.Add(entry, DefaultTimeout);
            }
        }
        private static AppStoreGenresCacheEntity _appStoreGenres;
        internal static AppStoreGenresCacheEntity AppStoreGenres
        {
            get { return _appStoreGenres ?? (_appStoreGenres = new AppStoreGenresCacheEntity(CacheOperations.Cache)); }
        }
    }
}
