using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Booking.API.Infrastructure.AutofacModules;
using Booking.Application.Booking;
using Booking.Application.Booking.Commands.CreateBooking;
using Booking.Application.Booking.Queries.GetBooking;
using Booking.Application.IntegrationEvents;
using Booking.Application.IntegrationEvents.Events;
using Booking.Domain.Booking;
using Booking.Persistence;
using Booking.Persistence.Repositories;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.MicroCouriers.BuildingBlocks.EventBusServiceBus;
using Microsoft.MicroCouriers.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.MicroCouriers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Booking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.
                AddCustomOptions(Configuration).

                AddIntegrationServices(Configuration)
                .AddCustomDbContext(Configuration).
                RegisterEventBus(Configuration).
                AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //Add Repos
            services.AddScoped<IBookingRespository, BookingRepository>();

            // Add MediatR and handlers
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddMediatR(typeof(GetBookingQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateBookingCommandHandler).GetTypeInfo().Assembly);

            var container = new ContainerBuilder();
            container.Populate(services);

            //container.RegisterModule(new MediatorModule());
            //container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("MicroCouriersDataBase")));

            return new AutofacServiceProvider(container.Build());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<PaymentProcessedIntegrationEvent, PaymentProcessedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();
            //eventBus.Subscribe<CustomerAddIntegrationEvent, CustomerAddIntegrationEventHandler>();

        }
    }

    static class CustomExtensionsMethods
    {

        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
               // sp => (DbConnection c) => new IntegrationEventLogService(c));

            //services.AddTransient<IBookingIntegrationEventService, BookingIntegrationEventService>();

            services.AddSingleton<IServiceBusPersisterConnection>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<ConfigurationSettings>>().Value;
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                var serviceBusConnection = new ServiceBusConnectionStringBuilder(settings.EventBusConnection);

                return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
            });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            // Add DbContext using SQL Server Provider
            //services.AddDbContext<BookingDbContext>(options =>
               // options.UseSqlServer(configuration.GetConnectionString("MicroCouriersDataBase")));

            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MicroCouriersDataBase"), x => x.MigrationsAssembly("Booking.Persistence")));
            /*
           services.AddEntityFrameworkSqlServer()
                  .AddDbContext<BookingDbContext>(options =>
                  {
                      options.UseSqlServer(configuration.GetConnectionString("MicroCouriersDataBase"),
                          sqlServerOptionsAction: sqlOptions =>
                          {
                              sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                              sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                          });
                  },
                      ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                  );


           services.AddDbContext<IntegrationEventLogContext>(options =>
           {
               options.UseSqlServer(configuration.GetConnectionString("MicroCouriersDataBase"),
                                    sqlServerOptionsAction: sqlOptions =>
                                    {
                                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                    });
           });
           */
            return services;
        }

        public static IServiceCollection RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                    eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<PaymentProcessedIntegrationEventHandler>();
            services.AddTransient<OrderStatusChangedIntegrationEventHandler>();

            return services;
        }

        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigurationSettings>(configuration);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }

    }


    public class ConfigurationSettings
    {
        public string EventBusConnection { get; set; }
    }
}
