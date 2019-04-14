using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.MicroCouriers.BuildingBlocks.EventBusServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
using Autofac;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus;
using System;
using MediatR;
using MediatR.Pipeline;
using Autofac.Extensions.DependencyInjection;
using Tracking.Domain.Interfaces;
using Tracking.Persistence.Repositories;
using Tracking.Application.IntegrationEvents;
using Tracking.Application.Interface;
using Tracking.Application.TrackingServices;
using StackExchange.Redis;
using Microsoft.ApplicationInsights.Extensibility;

namespace Tracking.API
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


            services.AddApplicationInsightsTelemetry(Configuration);

            //Add Repos


            //If Cache is available we will read the booking history from Cache
            if (Configuration.GetSection("cache:REDIS").Value != string.Empty)
            {
                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetSection("cache:REDIS").Value));
                services.AddScoped<ITrackingService, TrackingService>();
            }
            else
            {
                services.AddScoped<ITrackingService, TrackingSerivceWithoutCache>();
            }

            // Add MediatR and handlers
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));

            var eventStoreConnectionString = Configuration.GetConnectionString("EventStoreCN");
           // services.AddScoped<ITrackingRepository, TrackingRepository>(Configuration.GetConnectionString(""));

            //Event Store Initialization
            services.AddTransient<ITrackingRepository>((sp) =>
               new TrackingRepository(eventStoreConnectionString));

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ITrackingRepository trackingRepo,IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            var tConfig = app.ApplicationServices.GetRequiredService<TelemetryConfiguration>();
            tConfig.InstrumentationKey = config["ApplicationInsights:InstrumentationKey"];
            
            app.UseMvc();

            //Ensure Event Database exists
            trackingRepo.EnsureDatabase();


            ConfigureEventBus(app);
        }

        //Subscribe to event bus
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<PaymentProcessedIntegrationEvent, PaymentProcessedIntegrationEventHandler>();
            eventBus.Subscribe<BookingAddIntegrationEvent, BookingAddIntegrationEventHandler>();
            eventBus.Subscribe<OrderPickedIntegrationEvent, OrderPickedIntegrationEventHandler>();
            eventBus.Subscribe<OrderTransitIntegrationEvent, OrderTransitIntegrationEventHandler>();
            eventBus.Subscribe<OrderDeliveredIntegrationEvent, OrderDeliveredIntegrationEventHandler>();
        }

    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
            services.AddTransient<BookingAddIntegrationEventHandler>();
            services.AddTransient<OrderPickedIntegrationEventHandler>();
            services.AddTransient<OrderTransitIntegrationEventHandler>();
            services.AddTransient<OrderDeliveredIntegrationEventHandler>();


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
