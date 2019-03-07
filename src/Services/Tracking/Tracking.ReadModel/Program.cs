using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using Tracking.ReadModel.Database;

namespace Tracking.ReadModel
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        const string SecretName = "microcouriers.redis.cache.windows.net,abortConnect=false,ssl=true,password=microcouriers.redis.cache.windows.net:6380,password=UaIei+7JYi+3TinfolGRob5lIQ4HZ1Uk8IKyIcHcjfc=,ssl=True,abortConnect=False";

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            Configuration = builder.Build();
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = Configuration[SecretName];
            return ConnectionMultiplexer.Connect(SecretName);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        static void Main(string[] args)
        {          
            EventStore es = new EventStore();
            es.GetAllBookings().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}
