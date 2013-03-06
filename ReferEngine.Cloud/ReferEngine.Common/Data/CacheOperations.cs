﻿using System;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.Data
{
    public static class CacheKeyFormat
    {
        internal const string Person = "person-fbId-{0}";
        internal const string FacebookOperations = "fb-operations-{0}";
        internal const string AppPackage = "app-package-{0}";
        internal const string AppId = "app-id-{0}";
        internal const string AppScreenshotIdDesc = "appscreenshot-id-desc-{0}{1}";
        internal const string UserId = "user-id-{0}";
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

        public static void AddApp(string packageFamilyName, App app)
        {
            String key = String.Format(CacheKeyFormat.AppPackage, packageFamilyName);
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

        public static User GetUser(int id)
        {
            String key = String.Format(CacheKeyFormat.UserId, id);
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
            return cached == null ? null : (User)cached;
        }

        public static void AddUser(User user)
        {
            if (user == null) return;
            String key = String.Format(CacheKeyFormat.UserId, user.Id);
            CachePutSafe(key, user);
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