// --------------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.15">Created</change>
// </history>
namespace Fanex.TicketControl.Core.Web
{
    using System;

    using Fanex.Caching;

    /// <summary>
    /// Cache manager.
    /// </summary>
    public static class CacheManager
    {
        private static readonly ICache _CacheManager;

        static CacheManager()
        {
            _CacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public static object Get(string key)
        {
            try
            {
                return _CacheManager.Get<object>(key);
            }
            catch (Exception exception)
            {
                throw new Exception("The cache has an invalid key.", exception);
            }
        }

        public static T Get<T>(string key)
        {
            try
            {
                return _CacheManager.Get<T>(key);
            }
            catch (Exception exception)
            {
                throw new Exception("The cache has an invalid key.", exception);
            }
        }

        public static void Remove(string key)
        {
            _CacheManager.Remove(key);
        }

        public static void Set(string key, object value)
        {
            _CacheManager.Set(key, value);
        }

        public static void Set<T>(string key, T value)
        {
            _CacheManager.Set<T>(key, value);
        }

        public static void Set(string key, object value, TimeSpan timeSpanExpiration)
        {
            _CacheManager.Set(key, value, timeSpanExpiration);
        }

        public static void Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            _CacheManager.Set<T>(key, value, timeSpanExpiration);
        }

        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            _CacheManager.Set(key, value, absoluteExpiration);
        }

        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            _CacheManager.Set<T>(key, value, absoluteExpiration);
        }
    }
}