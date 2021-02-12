using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ECommerce.SharedLib.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ECommerce.Cart.Cache
{
    public class RedisCache : ICacheProvider
    {
        private readonly IDatabase _db;

        public RedisCache(IOptions<RedisSettings> redisSettings)
        {
            var settings = redisSettings.Value;
            var connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = {{settings.Host, settings.Port}},
                Password = settings.Password
            });

            _db = connection.GetDatabase();
        }

        public T Get<T>(string key)
        {
            var data = _db.StringGet(key);
            return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var data = await _db.StringGetAsync(key);
            return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data);
        }

        public bool Set(string key, object data, int expireSeconds)
        {
            var serializedData = JsonSerializer.Serialize(data);
            return _db.StringSet(key, serializedData, TimeSpan.FromSeconds(expireSeconds));
        }

        public Task<bool> SetAsync(string key, object data, int expireSeconds)
        {
            var serializedData = JsonSerializer.Serialize(data);
            return _db.StringSetAsync(key, serializedData, TimeSpan.FromSeconds(expireSeconds));
        }

        public bool Delete(string key)
        {
            return _db.KeyDelete(key);
        }

        public Task<bool> DeleteAsync(string key)
        {
            return _db.KeyDeleteAsync(key);
        }
    }
}