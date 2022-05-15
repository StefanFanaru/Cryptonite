using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Services.Portofolio;
using Cryptonite.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Services.Portofolio
{
    public class PortofolioSync : IPortofolioSync
    {
        private readonly IPortofolioRepository _portofolioRepository;
        private readonly IRepository _repository;

        public PortofolioSync(IRepository repository, IPortofolioRepository portofolioRepository)
        {
            _repository = repository;
            _portofolioRepository = portofolioRepository;
        }

        public async Task SyncPortofolios()
        {
            var buyEntries = await _repository.Query<BuyEntry>().ToListAsync();

            foreach (var entry in buyEntries)
            {
                await _portofolioRepository.IncreaseCryptocurrencyAmount(entry.UserId, entry.BoughtCryptocurrency, entry.BoughtAmount);
            }

            var tradeEntries = await _repository.Query<TradeEntry>().ToListAsync();

            foreach (var entry in tradeEntries)
            {
                await _portofolioRepository.IncreaseCryptocurrencyAmount(entry.UserId, entry.GainedCryptocurrency, entry.GainedAmount);
                await _portofolioRepository.DecreaseCryptocurrencyAmount(entry.UserId, entry.PaidCryptocurrency, entry.PaidAmount);
            }
        }
    }
}