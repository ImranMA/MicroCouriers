using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadModel.AzFn.Redis
{
    public class RedisCacheService 
    {       
        private readonly IDatabase _cache;
        private static ConnectionMultiplexer _connectionMultiplexer;

        static RedisCacheService()
        {
            var connection = Environment.GetEnvironmentVariable("REDIS");
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
        }

        //Get Cache Instance
        public RedisCacheService()
        {           
            _cache = _connectionMultiplexer.GetDatabase();
        }

        //Check if key exists
        public bool Exists(string key)
        {
            return _cache.KeyExists(key);
        }


        //Save into cache
        public async Task Save(string key, string value)
        {
           //We keep the event for 10 hours
           var ts = TimeSpan.FromHours(10);
           await _cache.StringSetAsync(key, value, ts);
        }

        //Get from Cache
        public string Get(string key)
        {
            return _cache.StringGet(key);
        }

        public async Task Remove(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        //Clear whole database
        public void Clear()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        //To Clear all cache
        public void ClearAllKeys()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            var server = _connectionMultiplexer.GetServer(endpoints.First());
            
            var keys = server.Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("Removing Key {0} from cache", key.ToString());
                _cache.KeyDelete(key);
            }
        }
    }
}
