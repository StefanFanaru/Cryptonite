using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Data.DataMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cryptonite.Infrastructure.Data.Common
{
    [ExcludeFromCodeCoverage]
    public static class DataExtensions
    {
        public static IServiceCollection AddAppDatabase<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            var migrationsAssembly = typeof(TContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }

        public static async Task InitializeDatabase<TContext>(this IServiceProvider serviceProvider,
            bool includeDataMigrations = true, int retryForAvailability = 0)
            where TContext : DbContext
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DataMigration>>();
            try
            {
                await serviceProvider.GetRequiredService<TContext>().Database.EnsureCreatedAsync();
                await serviceProvider.GetRequiredService<TContext>().Database.MigrateAsync();

                if (includeDataMigrations)
                {
                    serviceProvider.GetRequiredService<IDataMigrator>().MigrateData();
                    includeDataMigrations = false;
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability > 5)
                {
                    throw;
                }

                retryForAvailability++;
                await Task.Delay(2000 * retryForAvailability);
                logger.LogError(e.Message);
                logger.LogInformation($"Retrying database initialization. Retry number {retryForAvailability}");
                await serviceProvider.InitializeDatabase<TContext>(includeDataMigrations, retryForAvailability);
            }
        }

        public static async Task InitializeDatabases(this IServiceProvider serviceProvider, List<DbContext> dbContexts,
            bool includeDataMigrations = true, int retryForAvailability = 0)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DataMigration>>();
            try
            {
                if (includeDataMigrations)
                {
                    serviceProvider.GetRequiredService<IDataMigrator>().MigrateData();
                    includeDataMigrations = false;
                }

                foreach (var dbContext in dbContexts)
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability > 5)
                {
                    throw;
                }

                retryForAvailability++;
                await Task.Delay(2000 * retryForAvailability);
                logger.LogError(e.Message);
                logger.LogInformation($"Retrying database initialization. Retry number {retryForAvailability}");
                await serviceProvider.InitializeDatabases(dbContexts, includeDataMigrations, retryForAvailability);
            }
        }
    }
}