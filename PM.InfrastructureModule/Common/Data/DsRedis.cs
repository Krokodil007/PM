using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PM.InfrastructureModule.Common.App;
using StackExchange.Redis;

namespace PM.InfrastructureModule.Common.Data
{
    /// <summary>
    /// Redis Cache
    /// </summary>
    [UsedImplicitly]
    public class DsRedis
    {
        private static readonly IConfigurationRoot Config = AppSettingBuilder.GetAppSettings();

        /// <summary>
        /// Get Redis Entity Data
        /// </summary>
        public static T JsonToObjectGet<T>(string key)
        {
            IDatabase cache = Connection.GetDatabase();
            if (!ItemExists(key)) return default(T);
            var item = JsonConvert.DeserializeObject<T>(cache.StringGet($"{key}"));
            return item;
        }

        /// <summary>
        /// Get Redis Entity Data Async
        /// </summary>
        public static async Task<T> JsonToObjectGetAsync<T>(string key)
        {
            IDatabase cache = Connection.GetDatabase();
            var item = JsonConvert.DeserializeObject<T>(await cache.StringGetAsync($"{key}"));
            return item;
        }

        /// <summary>
        /// Get Redis Data
        /// </summary>
        public static string ItemGet(string key)
        {
            var cache = Connection.GetDatabase();

            if (!ItemExists(key)) return string.Empty;
            var item = key != null ? cache.StringGet($"{key}") : RedisValue.EmptyString;
            return item;
        }

        /// <summary>
        /// Get Redis Data Async
        /// </summary>
        public static async Task<string> ItemGetAsync(string key)
        {
            var cache = Connection.GetDatabase();

            var item = key != null ? await cache.StringGetAsync($"{key}") : RedisValue.EmptyString;
            return item;
        }

        /// <summary>
        /// Remove Redis Data
        /// </summary>
        public static void ItemRemove(string key)
        {
            var cache = Connection.GetDatabase();
            try
            {
                cache.KeyDelete($"{key}");
            }
            catch (RedisTimeoutException e)
            {

            }
        }

        /// <summary>
        /// Remove Redis Data
        /// </summary>
        public static async Task ItemRemoveAsync(string key)
        {
            var cache = Connection.GetDatabase();
            await cache.KeyDeleteAsync($"{key}");
        }

        /// <summary>
        /// Set Redis Data
        /// </summary>
        public static string ItemSet(string key, RedisValue value, int ttlMinData = 120)
        {
            var cache = Connection.GetDatabase();

            var item = key != null
                ? cache.StringSet($"{key}", value, TimeSpan.FromMinutes(ttlMinData))
                : RedisValue.EmptyString;
            return item;
        }

        /// <summary>
        /// Set Redis Data Async
        /// </summary>
        public static async Task<string> ItemSetAsync(string key, RedisValue value, int ttlMinData = 120)
        {
            var cache = Connection.GetDatabase();

            var item = key != null
                ? await cache.StringSetAsync($"{key}", value, TimeSpan.FromMinutes(ttlMinData))
                : RedisValue.EmptyString;
            return item;
        }

        /// <summary>
        /// Check Redis Data
        /// </summary>
        public static bool ItemExists(string key)
        {
            var cache = Connection.GetDatabase();

            var item = cache.KeyExists($"{key}", CommandFlags.DemandMaster);
            return item;
        }

        /// <summary>
        /// Check Redis Data Async
        /// </summary>
        public static async Task<bool> ItemExistsAsync(string key)
        {
            var cache = Connection.GetDatabase();

            var item = await cache.KeyExistsAsync($"{key}", CommandFlags.DemandMaster);
            return item;
        }

        /// <summary>
        /// Set Redis Json Data
        /// </summary>
        public static void ObjectToJsonSet<T>(T data, string key, int ttlMinData = 120)
        {
            if (string.IsNullOrEmpty(key)) return;

            var cache = Connection.GetDatabase();

            var resData = data != null ? JsonConvert.SerializeObject(data) : @"{}";
            cache.StringSet($"{key}", resData, TimeSpan.FromMinutes(ttlMinData));
        }

        /// <summary>
        /// Set Redis Json Data Async
        /// </summary>
        public static async Task ObjectToJsonSetAsync<T>(T data, string key, int ttlMinData = 120)
        {
            if (string.IsNullOrEmpty(key)) return;

            var cache = Connection.GetDatabase();

            var resData = data != null ? JsonConvert.SerializeObject(data) : @"{}";
            await cache.StringSetAsync($"{key}", resData, TimeSpan.FromMinutes(ttlMinData));
        }

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer
                .Connect(Config["AzureRedisCache"]));

        private static ConnectionMultiplexer Connection => LazyConnection.Value;
    }
}