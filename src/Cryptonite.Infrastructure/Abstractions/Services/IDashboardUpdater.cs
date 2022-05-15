using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.Services
{
    public interface IDashboardUpdater
    {
        Task Update();
    }
}