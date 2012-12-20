using System;
using Microsoft.ApplicationServer.Caching;
using ReferLib;

namespace ReferEngineWeb.DataAccess
{
    public class CacheOperations
    {
        private const string PersonCacheKeyFormat = "person-fbId-{0}";
        private const string FacebookOperationsCacheKeyFormat = "{0}-fb-operations";
        private const string AppPackageKeyFormat = "app-package-{0}";
        private const string AppIdKeyFormat = "app-id-{0}";
        private static DataCache _cache = null;
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
                    catch (DataCacheException ex)
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
            String key = String.Format(PersonCacheKeyFormat, facebookId);
            object cached = Cache.Get(key);
            return cached == null ? null : (Person) cached;
        }

        public static void AddPerson(Person person)
        {
            String key = String.Format(PersonCacheKeyFormat, person.FacebookId);
            Cache.Put(key, person);
        }

        public static App GetApp(long id)
        {
            String key = String.Format(AppIdKeyFormat, id);
            object cached = Cache.Get(key);
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(long id, App app)
        {
            String key = String.Format(AppIdKeyFormat, id);
            Cache.Put(key, app);
        }

        public static App GetApp(string packageFamilyName)
        {
            String key = String.Format(AppPackageKeyFormat, packageFamilyName);
            object cached = Cache.Get(key);
            return cached == null ? null : (App)cached;
        }

        public static void AddApp(string packageFamilyName, App app)
        {
            String key = String.Format(AppPackageKeyFormat, packageFamilyName);
            Cache.Put(key, app);
        }

        public static void AddFacebookOperations(string token, FacebookOperations facebookOperations)
        {
            string key = string.Format(FacebookOperationsCacheKeyFormat, token);
            Cache.Put(key, facebookOperations);
        }

        public static FacebookOperations GetFacebookOperations(string token)
        {
            string key = string.Format(FacebookOperationsCacheKeyFormat, token);
            object cached = Cache.Get(key);
            if (cached != null)
            {
                return (FacebookOperations)cached;
            }

            return null;
        }
    }
}