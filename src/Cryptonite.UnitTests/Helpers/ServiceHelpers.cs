using System;
using System.Collections.Generic;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data;
using Cryptonite.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Extensions;

namespace Cryptonite.UnitTests.Helpers
{
    public static class ServiceHelpers
    {
        public static IRepository CreateRepository()
        {
            return CreateRepository<IRepository, Repository>();
        }

        public static TAbstraction CreateRepository<TAbstraction, TImplementation>()
            where TAbstraction : class where TImplementation : class, TAbstraction
        {
            var provider = new ServiceCollection()
                .AddScoped<TAbstraction, TImplementation>()
                .AddDbContext<CryptoniteContext>(options =>
                {
                    options
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .ConfigureWarnings(x => { x.Ignore(InMemoryEventId.TransactionIgnoredWarning); });
                }, ServiceLifetime.Transient)
                .BuildServiceProvider();

            EntityFrameworkManager.PreBulkUpdate = (context, o) => context.Entry(o).State = EntityState.Detached;
            EntityFrameworkManager.PreBulkDelete = (context, o) => context.Entry(o).State = EntityState.Detached;
            EntityFrameworkManager.ContextFactory = _ => provider.GetRequiredService<CryptoniteContext>();
            BatchUpdateManager.InMemoryDbContextFactory = () => provider.GetRequiredService<CryptoniteContext>();

            return provider.GetRequiredService<TAbstraction>();
        }

        public static IServiceProvider BuildServiceProvider()
        {
            var provider = new ServiceCollection().AddHandlers(typeof(InfrastructureAssembly).Assembly).WithPipelineValidation();
            return provider.BuildServiceProvider();
        }

        public static IMemoryCache CreateMemoryCache()
        {
            var provider = new ServiceCollection()
                .AddMemoryCache()
                .BuildServiceProvider();

            return provider.GetRequiredService<IMemoryCache>();
        }

        public static IConfiguration CreateConfiguration(Dictionary<string, string> configuration = null)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(configuration)
                .Build();
        }
    }
}