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

        public RedisCacheService()
        {           
            _cache = _connectionMultiplexer.GetDatabase();
        }

        public bool Exists(string key)
        {
            return _cache.KeyExists(key);
        }

        public async Task Save(string key, string value)
        {
         
           var ts = TimeSpan.FromHours(10);
           await _cache.StringSetAsync(key, value, ts);

            /*RedisValue token = Environment.MachineName;
            if (_cache.LockTake(key, token, ts))
            {
                try
                {
                    _cache.StringSet(key, value, ts);
                }
                finally
                {
                    _cache.LockRelease(key, token);
                }
            }*/
        }

        public string Get(string key)
        {
            return _cache.StringGet(key);
        }

        public async Task Remove(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public void Clear()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public void ClearAllKeys()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints();
            var server = _connectionMultiplexer.GetServer(endpoints.First());
            //FlushDatabase didn't work for me: got error admin mode not enabled error
            //server.FlushDatabase();
            var keys = server.Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("Removing Key {0} from cache", key.ToString());
                _cache.KeyDelete(key);
            }
        }
    }
}
