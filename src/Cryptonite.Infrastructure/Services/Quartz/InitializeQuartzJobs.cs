using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Quartz;

namespace Cryptonite.Infrastructure.Services.Quartz
{
    [ExcludeFromCodeCoverage]
    public class InitializeQuartzJobs : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.CompletedTask;
        }
    }
}