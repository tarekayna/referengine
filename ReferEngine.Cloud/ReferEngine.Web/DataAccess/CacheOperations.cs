using System;
using Microsoft.ApplicationServer.Caching;
using ReferEngine.Common.Models;

namespace ReferEngine.Web.DataAccess
{
    public static class CacheKeyFormat
    {
        internal const string Person = "person-fbId-{0}";
        internal const string FacebookOperations = "fb-operations-{0}";
        internal const string AppPackage = "app-package-{0}";
        internal const string AppId = "app-id-{0}";
        internal const string AppScreenshotIdDesc= "appscreenshot-id-desc-{0}{1}";
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

        public static void AddAppAuthorization(AppAuthorization appAuthorization, TimeSpan expiresIn)
        {
            Cache.Put(appAuthorization.Token, appAuthorization, expiresIn);
        }

        public static AppAuthorization GetAppAuthorization(string token, string userHostAddress)
        {
            object cached = Cache.Get(token);
            if (cached != null)
            {
                AppAuthorization appAuthorization = (AppAuthorization)cached;
                if (appAuthorization.UserHostAddress.Equals(userHostAddress, StringComparison.OrdinalIgnoreCase))
                {
                    return appAuthorization;
                }
            }
            return null;
        }

        public static Person GetPerson(Int64 facebookId)
        {
            String key = String.Format(CacheKeyFormat.Person, facebookId);
            object cached = Cache.Get(key);
            return cached == null ? null : (Person) cached;
        }

        public static void AddPerson(Person person)
        {
            String key = String.Format(CacheKeyFormat.Person, person.FacebookId);
            Cache.Put(key, person);
        }

        public static App GetApp(long id)
        {
            String key = String.Format(CacheKeyFormat.AppId, id);
            object cached = Cache.Get(key);
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(long id, App app)
        {
            String key = String.Format(CacheKeyFormat.AppId, id);
            Cache.Put(key, app);
        }

        public static App GetApp(string packageFamilyName)
        {
            String key = String.Format(CacheKeyFormat.AppPackage, packageFamilyName);
            object cached = Cache.Get(key);
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(string packageFamilyName, App app)
        {
            String key = String.Format(CacheKeyFormat.AppPackage, packageFamilyName);
            Cache.Put(key, app);
        }

        public static void AddFacebookOperations(string token, FacebookOperations facebookOperations)
        {
            string key = string.Format(CacheKeyFormat.FacebookOperations, token);
            Cache.Put(key, facebookOperations);
        }

        public static FacebookOperations GetFacebookOperations(string token)
        {
            string key = string.Format(CacheKeyFormat.FacebookOperations, token);
            object cached = Cache.Get(key);
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
            Cache.Put(key, screenshot);
            return screenshot;
        }
    }
}