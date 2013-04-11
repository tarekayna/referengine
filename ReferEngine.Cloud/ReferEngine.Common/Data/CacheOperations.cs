using System;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;
using ReferEngine.Common.Models;
using ReferEngine.Common.ViewModels.AppStore.Windows;

namespace ReferEngine.Common.Data
{
    internal class CachedEntity<T>
    {
        private readonly DataCache _cache;
        private readonly string _keyFormat;
        protected readonly TimeSpan? DefaultTimeout = null;

        public CachedEntity(DataCache cache, string keyFormat)
        {
            _cache = cache;
            _keyFormat = keyFormat;
        }

        protected void Remove(params string[] keyParams)
        {
            String key = String.Format(_keyFormat, keyParams);
            _cache.Remove(key);
        }

        protected T Get(params string[] keyParams)
        {
            String key = String.Format(_keyFormat, keyParams);
            object cached = null;
            try
            {
                cached = _cache.Get(key);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                // It's ok, just retreive from the database
            }
            return (T)cached;
        }

        protected void Add(T obj, TimeSpan? timeout = null, params string[] keyParams)
        {
            if (obj == null) return;
            String key = String.Format(_keyFormat, keyParams);
            if (timeout.HasValue)
            {
                CachePutSafe(key, obj, timeout.Value);
            }
            else
            {
                CachePutSafe(key, obj);
            }
        }

        private void CachePutSafe(string key, T value)
        {
            try
            {
                _cache.Put(key, value);
            }
            catch (Exception t)
            {
                Trace.TraceError(t.Message);
            }
        }
        private void CachePutSafe(string key, T value, TimeSpan timeout)
        {
            try
            {
                _cache.Put(key, value, timeout);
            }
            catch (Exception t)
            {
                Trace.TraceError(t.Message);
            }
        }
    }

    internal class UserCacheEntity : CachedEntity<User>
    {
        public static string KeyFormat = "user-id-{0}";
        public UserCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(User user)
        {
            base.Remove(user.Id.ToString());
        }

        public User Get(long id)
        {
            return base.Get(id.ToString());
        }

        public void Add(User user)
        {
            base.Add(user, DefaultTimeout, user.Id.ToString());
        }
    }

    internal class AppByIdCacheEntity : CachedEntity<App>
    {
        public static string KeyFormat = "app-id-{0}";
        public AppByIdCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(App app)
        {
            base.Remove(app.Id.ToString());
        }

        public App Get(long id)
        {
            return base.Get(id.ToString());
        }

        public void Add(App app)
        {
            base.Add(app, DefaultTimeout, app.Id.ToString());
        }
    }

    internal class AppByPlatformAndNameCacheEntity : CachedEntity<App>
    {
        public static string KeyFormat = "app-plat-name-{0}{1}";
        public AppByPlatformAndNameCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(App app)
        {
            base.Remove(app.Platform, app.Name);
        }

        public App Get(string platform, string name)
        {
            return base.Get(platform, name);
        }

        public void Add(App app)
        {
            base.Add(app, DefaultTimeout, app.Platform, app.Name);
        }
    }

    internal class WindowsAppViewModelByNameCacheEntity : CachedEntity<WindowsAppViewModel>
    {
        public static string KeyFormat = "WindowsAppViewModel-name-{0}";
        public WindowsAppViewModelByNameCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(WindowsAppViewModel viewModel)
        {
            base.Remove(viewModel.WindowsAppStoreInfo.Name);
        }

        public WindowsAppViewModel Get(string name)
        {
            return base.Get(name);
        }

        public void Add(WindowsAppViewModel viewModel)
        {
            base.Add(viewModel, DefaultTimeout, viewModel.WindowsAppStoreInfo.Name);
        }
    }

    internal class WindowsCategoryViewModelCacheEntity : CachedEntity<WindowsCategoryViewModel>
    {
        public static string KeyFormat = "WindowsCategoryViewModel-name-{0}";
        public WindowsCategoryViewModelCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(WindowsCategoryViewModel viewModel)
        {
            base.Remove(viewModel.Category.Name);
        }

        public WindowsCategoryViewModel Get(string name)
        {
            return base.Get(name);
        }

        public void Add(WindowsCategoryViewModel viewModel)
        {
            base.Add(viewModel, DefaultTimeout, viewModel.Category.Name);
        }
    }

    internal class AppByPackageAndVerificationCacheEntity : CachedEntity<App>
    {
        public static string KeyFormat = "app-pkgver-{0}{1}";
        public AppByPackageAndVerificationCacheEntity(DataCache cache) : base(cache, KeyFormat) { }

        public void Remove(App app)
        {
            base.Remove(app.PackageFamilyName, app.VerificationCode);
        }

        public App Get(string packageFamilyName, string verificationCode)
        {
            return base.Get(packageFamilyName, verificationCode);
        }

        public void Add(App app)
        {
            base.Add(app, DefaultTimeout, app.PackageFamilyName, app.VerificationCode);
        }
    }

    internal static class CacheKeyFormat
    {
        internal const string Person = "person-fbId-{0}";
        internal const string FacebookOperations = "fb-operations-{0}";
        internal const string IpAddress = "ip-{0}";
        internal const string AppAutoShowOptions = "app-auto-show-{0}";
    }

    internal static class CacheTimeoutValues
    {
        internal static TimeSpan App = TimeSpan.FromDays(1);
    }

    internal class CacheOperations
    {
        private static DataCache _cache;
        public static DataCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    try
                    {
                        _cache = new DataCache("default");
                    }
                    catch (DataCacheException)
                    {
                    }
                }
                return _cache;
            }
        }

        private static UserCacheEntity _user;
        public static UserCacheEntity User
        {
            get { return _user ?? (_user = new UserCacheEntity(Cache)); }
        }

        private static AppByIdCacheEntity _appById;
        public static AppByIdCacheEntity AppById
        {
            get { return _appById ?? (_appById = new AppByIdCacheEntity(Cache)); }
        }

        private static AppByPlatformAndNameCacheEntity _appByPlatformAndName;
        public static AppByPlatformAndNameCacheEntity AppByPlatformAndName
        {
            get { return _appByPlatformAndName ?? (_appByPlatformAndName = new AppByPlatformAndNameCacheEntity(Cache)); }
        }

        private static WindowsAppViewModelByNameCacheEntity _windowsAppViewModelByName;
        public static WindowsAppViewModelByNameCacheEntity WindowsAppViewModelByName
        {
            get { return _windowsAppViewModelByName ?? (_windowsAppViewModelByName = new WindowsAppViewModelByNameCacheEntity(Cache)); }
        }

        private static WindowsCategoryViewModelCacheEntity _windowsCategoryViewModel;
        public static WindowsCategoryViewModelCacheEntity WindowsCategoryViewModel
        {
            get { return _windowsCategoryViewModel ?? (_windowsCategoryViewModel = new WindowsCategoryViewModelCacheEntity(Cache)); }
        }

        private static AppByPackageAndVerificationCacheEntity _appByPackageAndVerification;
        public static AppByPackageAndVerificationCacheEntity AppByPackageAndVerification
        {
            get { return _appByPackageAndVerification ?? (_appByPackageAndVerification = new AppByPackageAndVerificationCacheEntity(Cache)); }
        }

        private static void CachePutSafe(string key, object value, TimeSpan timeout)
        {
            try
            {
                Cache.Put(key, value, timeout);
            }
            catch (Exception t)
            {
                Trace.TraceError(t.Message);
            }
        }

        private static void CachePutSafe(string key, object value)
        {
            try
            {
                Cache.Put(key, value);
            }
            catch (Exception t)
            {
                Trace.TraceError(t.Message);
            }
        }

        public static void AddAppAuthorization(AppAuthorization appAuthorization, TimeSpan expiresIn)
        {
            CachePutSafe(appAuthorization.Token, appAuthorization, expiresIn);
        }

        public static AppAuthorization GetAppAuthorization(string token)
        {
            object cached = null;
            try
            {
                cached = Cache.Get(token);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return cached != null ? (AppAuthorization) cached : null;
        }

        public static Person GetPerson(Int64 facebookId)
        {
            String key = String.Format(CacheKeyFormat.Person, facebookId);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return cached == null ? null : (Person) cached;
        }

        public static void AddPerson(Person person)
        {
            String key = String.Format(CacheKeyFormat.Person, person.FacebookId);
            CachePutSafe(key, person);
        }

        public static void AddFacebookOperations(string token, FacebookOperations facebookOperations)
        {
            string key = string.Format(CacheKeyFormat.FacebookOperations, token);
            CachePutSafe(key, facebookOperations);
        }

        public static FacebookOperations GetFacebookOperations(string token)
        {
            string key = string.Format(CacheKeyFormat.FacebookOperations, token);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            if (cached != null)
            {
                return (FacebookOperations)cached;
            }

            return null;
        }

        public static IpAddressLocation GetIpAddressLocation(string ipAddress)
        {
            String key = String.Format(CacheKeyFormat.IpAddress, ipAddress);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                // It's ok, just retreive from the database
            }
            return cached == null ? null : (IpAddressLocation)cached;
        }

        public static void SetIpAddressLocation(IpAddressLocation ipAddressLocation)
        {
            if (ipAddressLocation == null) return;
            String key = String.Format(CacheKeyFormat.IpAddress, ipAddressLocation.IpAddress);
            CachePutSafe(key, ipAddressLocation);
        }

        public static AppAutoShowOptions GetAppAutoShowOptions(long appId)
        {
            String key = String.Format(CacheKeyFormat.AppAutoShowOptions, appId);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                // It's ok, just retreive from the database
            }
            return cached == null ? null : (AppAutoShowOptions)cached;
        }

        public static void SetAppAutoShowOptions(AppAutoShowOptions appAutoShowOptions)
        {
            if (appAutoShowOptions == null) return;
            String key = String.Format(CacheKeyFormat.AppAutoShowOptions, appAutoShowOptions.AppId);
            CachePutSafe(key, appAutoShowOptions);
        }
    }
}