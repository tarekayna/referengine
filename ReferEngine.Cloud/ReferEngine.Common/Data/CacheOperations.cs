﻿using System;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Data
{
    public class CachedEntity<T>
    {
        private readonly DataCache _cache;
        private readonly string _keyFormat;

        public CachedEntity(DataCache cache, string keyFormat)
        {
            _cache = cache;
            _keyFormat = keyFormat;
        }

        protected void Clear(params string[] keyParams)
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

    public class UserCacheEntity : CachedEntity<User>
    {
        public static string KeyFormat = "user-id-{0}";
        public UserCacheEntity(DataCache cache) : base(cache, KeyFormat) {}

        public void Clear(User user)
        {
            base.Clear(user.Id.ToString());
        }

        public User Get(long id)
        {
            return base.Get(id.ToString());
        }

        public void Add(User user)
        {
            base.Add(user, null, user.Id.ToString());
        }
    }

    public static class CacheKeyFormat
    {
        internal const string Person = "person-fbId-{0}";
        internal const string FacebookOperations = "fb-operations-{0}";
        internal const string AppPackage = "app-package-{0}";
        internal const string AppPackageAndVerification = "app-pkgver-{0}{1}";
        internal const string AppId = "app-id-{0}";
        internal const string AppScreenshotIdDesc = "appscreenshot-id-desc-{0}{1}";
        internal const string IpAddress = "ip-{0}";
        internal const string AppAutoShowOptions = "app-auto-show-{0}";
    }

    public static class CacheTimeoutValues
    {
        internal static TimeSpan App = TimeSpan.FromDays(1);
    }

    public class CacheOperations
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

        public static App GetApp(long id)
        {
            String key = String.Format(CacheKeyFormat.AppId, id);
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
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(long id, App app)
        {
            String key = String.Format(CacheKeyFormat.AppId, id);
            CachePutSafe(key, app);
        }

        public static void RemoveApp(long id, string packageFamilyName)
        {
            Cache.Remove(String.Format(CacheKeyFormat.AppId, id));
            Cache.Remove(String.Format(CacheKeyFormat.AppPackage, packageFamilyName));
        }

        public static App GetApp(string packageFamilyName)
        {
            String key = String.Format(CacheKeyFormat.AppPackage, packageFamilyName);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return cached == null ? null : (App)cached;
        }

        public static App GetApp(string packageFamilyName, string appVerificationCode)
        {
            String key = String.Format(CacheKeyFormat.AppPackageAndVerification, packageFamilyName, appVerificationCode);
            object cached = null;
            try
            {
                cached = Cache.Get(key);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(string packageFamilyName, App app)
        {
            String key = String.Format(CacheKeyFormat.AppPackage, packageFamilyName);
            CachePutSafe(key, app, CacheTimeoutValues.App);
        }

        public static void AddApp(string packageFamilyName, string appVerificationCode, App app)
        {
            String key = String.Format(CacheKeyFormat.AppPackageAndVerification, packageFamilyName, appVerificationCode);
            CachePutSafe(key, app, CacheTimeoutValues.App);
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

        public static AppScreenshot GetAppScreenshot(long appId, string description)
        {
            String key = String.Format(CacheKeyFormat.AppScreenshotIdDesc, appId, description);
            object cached = Cache.Get(key);
            if (cached != null)
            {
                return (AppScreenshot) cached;
            }

            AppScreenshot screenshot = DatabaseOperations.GetAppScreenshot(appId, description);
            CachePutSafe(key, screenshot);
            return screenshot;
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