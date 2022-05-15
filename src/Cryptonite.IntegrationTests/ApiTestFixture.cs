using System.IO;
using Cryptonite.API;
using Cryptonite.IntegrationTests.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cryptonite.IntegrationTests
{
    public class ApiTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", false)
                .Build();

            builder.ConfigureServices(services =>
            {
                services.Configure<RouteOptions>(configuration);
                services.AddTransient<RuntimeMiddlewareService>();
            });
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<StartupTest>(); }).ConfigureAppConfiguration(
                    (context, builder) => builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.test.json", false));
        }
    }
}