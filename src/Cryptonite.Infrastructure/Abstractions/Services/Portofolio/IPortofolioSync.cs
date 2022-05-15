using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.Services.Portofolio
{
    public interface IPortofolioSync
    {
        Task SyncPortofolios();
    }
}