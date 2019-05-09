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
using Microsoft.ApplicationInsights.Extensibility;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Booking.Domain.AggregatesModel.BookingAggregate;

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


            services.AddApplicationInsightsTelemetry(Configuration);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Booking API", Version = "v1" });
            });

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

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var tConfig = app.ApplicationServices.GetRequiredService<TelemetryConfiguration>();
            tConfig.InstrumentationKey = config["ApplicationInsights:InstrumentationKey"];// "dbd67a2f-a911-4d69-be31-e7c0b53b248d";

            app.UseMvc();
            ConfigureEventBus(app);


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            SetupAutoMapper();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API");
            });
        }

        //Subscribe to Events
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<PaymentProcessedIntegrationEvent, PaymentProcessedIntegrationEventHandler>();
            eventBus.Subscribe<OrderStatusChangedIntegrationEvent, OrderStatusChangedIntegrationEventHandler>();

        }

        private void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BookingOrder, BookingOrderDTO>();               
            });
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
            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MicroCouriersDataBase"), x => x.MigrationsAssembly("Booking.Persistence")));

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
