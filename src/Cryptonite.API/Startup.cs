using System;
using Cryptonite.API.Asp.Middleware.Exceptions;
using Cryptonite.API.Configuration;
using Cryptonite.API.Services.SignalR;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data;
using Cryptonite.Infrastructure.Data.Common;
using Cryptonite.Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Cryptonite.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfiguration = configuration;
            Environment = configuration.GetSection("Settings:Environment").Value;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfiguration { get; private set; }
        public static string Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAppServices()
                .AddAuth()
                .AddMemoryCache();
            services.AddControllers().AddApplicationPart(typeof(Startup).Assembly).AddNewtonsoftJson(options =>
            {
                options.AllowInputFormatterExceptionMessages = true;
                options.SerializerSettings.Converters.Add(new JsonMapper.UtcDateTimeConverter());
                options.SerializerSettings.Converters.Add(new OperationResultConverter());
            });

            services.AddSignalR();
            HumanizerInitializer.Initialize();
            services.AddHandlers(typeof(InfrastructureAssembly).Assembly).WithPipelineValidation();
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddDbContext<CryptoniteContext>(options =>
            {
                options
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureDevelopmentServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddAppDatabase<CryptoniteContext>(Configuration.GetConnectionString("CryptoniteSql"));
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    Configuration.GetSection("ApplicationUrls:AngularClient").Value);

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            });

            if (env.IsDevelopment())
            {
                app.UseSerilogRequestLogging();
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionMiddleware(env.IsProduction());

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();
            ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ClientEventHub>("/hubs/client-events");
            });
        }
    }
}