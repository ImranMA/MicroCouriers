using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shipping.Persistence;
using Polly;

namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create/Migrate the database
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<ShippingDbContext>();

                    var concreteContext = (ShippingDbContext)context;

                    //Retry logic for DB connectivity
                    Policy
                       .Handle<Exception>()
                       .WaitAndRetry(5, r => TimeSpan.FromSeconds(10))
                       .Execute(() => concreteContext.Database.Migrate());

                }
                catch (Exception)
                {

                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)               
                .UseStartup<Startup>();
    }
}
