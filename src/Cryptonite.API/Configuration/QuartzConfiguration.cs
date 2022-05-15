using Cryptonite.Core.Common;
using Cryptonite.Infrastructure.Services.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Cryptonite.API.Configuration
{
    public static class QuartzConfiguration
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.SchedulerName = "CryptoniteScheduler";
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                q.UseTimeZoneConverter();

                q.AddJob<InitializeQuartzJobs>(JobKey.Create("InitializeQuartzJobs"), j => j.StoreDurably());

                q.AddTrigger(t => t
                    .WithIdentity(new TriggerKey("InitializeQuartzJobsTrigger"))
                    .ForJob(JobKey.Create("InitializeQuartzJobs"))
                    .StartAt(TimeProvider.UtcNow.AddSeconds(5))
                );

                q.UseMicrosoftDependencyInjectionScopedJobFactory();
            });
            services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });
            return services;
        }
    }
}