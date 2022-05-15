using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using Cryptonite.API.Asp;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.API.Services.SignalR;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Cryptonite.Infrastructure.Abstractions.Portofolio;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.Services.Portofolio;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Data;
using Cryptonite.Infrastructure.Data.Common;
using Cryptonite.Infrastructure.Data.DataMigrations;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Queries.Portofolio.Distribution;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.Today;
using Cryptonite.Infrastructure.Services.Binance.Apis;
using Cryptonite.Infrastructure.Services.Binance.Sockets;
using Cryptonite.Infrastructure.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Services.Portofolio;
using Cryptonite.Infrastructure.Services.UserSettingsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace Cryptonite.API.Configuration
{
    public static class ApiConfiguration
    {
        private static readonly IConfiguration Configuration = Startup.StaticConfiguration;
        private static readonly Assembly InfrastructureAssembly = Assembly.GetAssembly(typeof(InfrastructureAssembly));

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IPortofolioRepository, PortofolioRepository>();
            services.AddScoped<IDataMigrator, DataMigrator>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserInfo, AspUserInfo>();

            services.AddScoped<IBinanceClient, BinanceClient>();
            services.AddScoped<IBinanceKlines, BinanceKlines>();
            services.AddSingleton<IBinanceTickers, BinanceTickers>();
            services.AddSingleton<IBinanceSocket, BinanceSocket>();
            services.AddSingleton<IMiniTickerBehaviour, MiniTickerBehaviour>();

            services.AddScoped<ICurrencyLayerService, CurrencyLayerService>();
            services.AddScoped<ICurrencyLayerClient, CurrencyLayerClient>();

            services.AddScoped<IPortofolioSync, PortofolioSync>();
            services.AddScoped<IDistributionService, DistributionService>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();
            services.AddScoped<TodayResultQueryHandler>();
            services.AddScoped<DistributionQueryHandler>();
            services.AddScoped<AllTimeResultQueryHandler>();

            services.AddSingleton<IClientEventSender, ClientEventSender>();
            services.AddSingleton<ISignalRConnectionManager, SignalRConnectionManager>();

            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
        {
            services.AddScopedImplementationsOf<IJob>(InfrastructureAssembly);
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var authority = Configuration["ApplicationUrls:IdentityServer"];

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authority;
                    options.Audience = "cryptonite";
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerValidator =
                            (issuer, token, parameters) => authority // to support Docker internal network
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("scope", "cryptonite_full")
                    .Build();

                options.AddPolicy("AdminOnly",
                    policyBuilder => policyBuilder
                        .RequireAuthenticatedUser()
                        .RequireClaim("role", "Administrator"));
            });

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
                    ConfigureJwtBearerOptions>());

            return services;
        }

        public static async Task InitializeApp(this IServiceProvider serviceProvider)
        {
            await serviceProvider.InitializeDatabase<CryptoniteContext>();
            var binanceClient = serviceProvider.GetRequiredService<IBinanceClient>();
            var binanceTickers = serviceProvider.GetRequiredService<IBinanceTickers>();
            var currencyLayerService = serviceProvider.GetRequiredService<ICurrencyLayerService>();

            await currencyLayerService.GetCurrentQuotes();
            binanceTickers.InitializeTickers(await binanceClient.GetTickers());
        }
    }
}