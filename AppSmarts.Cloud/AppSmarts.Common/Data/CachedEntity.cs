using System;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;

namespace AppSmarts.Common.Data
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
}