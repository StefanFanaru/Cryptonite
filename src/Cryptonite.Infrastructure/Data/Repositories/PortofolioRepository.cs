using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Entities;
using Cryptonite.Core.Exceptions;
using Cryptonite.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Cryptonite.Infrastructure.Data.Repositories
{
    public class PortofolioRepository : EfRepository<CryptoniteContext>, IPortofolioRepository
    {
        private readonly IMemoryCache _cache;

        public PortofolioRepository(CryptoniteContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task IncreaseCryptocurrencyAmount(string userId, string cryptocurrencySymbol, decimal amount, DateTime? time = null)
        {
            await UpdateCryptocurrency(userId, cryptocurrencySymbol, amount, false, time);
        }

        public async Task DecreaseCryptocurrencyAmount(string userId, string cryptocurrencySymbol, decimal amount, DateTime? time = null)
        {
            await UpdateCryptocurrency(userId, cryptocurrencySymbol, amount, true, time);
        }

        public async Task<Portofolio> RetrieveAsync(string userId)
        {
            return await TryGetCacheAsync(CacheKeys.Portofolio(userId), async () =>
                await Query<Portofolio>().AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync()
            );
        }

        public async Task<List<PortofolioCryptocurrency>> RetrieveCurrenciesAsync(string userid)
        {
            return await TryGetCacheAsync(CacheKeys.PortofolioCurrencies(userid), async () =>
                await Query<PortofolioCryptocurrency>().AsNoTracking()
                    .Where(x => x.Portofolio.UserId == userid)
                    .ToListAsync());
        }

        private async Task UpdateCryptocurrency(string userId, string cryptocurrencySymbol, decimal amount, bool isDecrease,
            DateTime? time = null)
        {
            _cache.Remove(CacheKeys.Portofolio(userId));
            await AddPortofolioIfMissingAsync(userId);

            var portofolioId = await Query<Portofolio>()
                .Where(x => x.UserId == userId).Select(x => x.Id).FirstAsync();

            var baseQuery = Query<PortofolioCryptocurrency>()
                .Where(x => x.Portofolio.UserId == userId)
                .Where(x => x.PortofolioId == portofolioId);

            var containsCryptocurrency = await baseQuery.AnyAsync(x => x.Symbol == cryptocurrencySymbol);
            if (!containsCryptocurrency)
            {
                if (isDecrease)
                {
                    throw new BusinessException("Trying to decrease cryptocurrency not existent in portofolio");
                }

                await AddCryptocurrencyAsync(portofolioId, cryptocurrencySymbol, amount, time);
                await IncreaseTransactionsAsync(userId);
                return;
            }

            await baseQuery.Where(x => x.Symbol == cryptocurrencySymbol)
                .UpdateFromQueryAsync(x => new PortofolioCryptocurrency
                {
                    UpdatedAt = time ?? TimeProvider.UtcNow,
                    Amount = isDecrease ? x.Amount - amount : x.Amount + amount
                });

            await Query<PortofolioCryptocurrency>()
                .Where(x => x.Amount == 0m).DeleteFromQueryAsync();

            await IncreaseTransactionsAsync(userId);
        }

        private async Task IncreaseTransactionsAsync(string userId)
        {
            await AddPortofolioIfMissingAsync(userId);
            await Query<Portofolio>().Where(x => x.UserId == userId).UpdateFromQueryAsync(x => new Portofolio
            {
                Transactions = x.Transactions + 1,
                LastTransactionAt = TimeProvider.UtcNow
            });
            await SaveAsync();
        }

        private async Task AddCryptocurrencyAsync(string portofolioId, string cryptocurrencySymbol, decimal amount,
            DateTime? insertedAt = null)
        {
            await InsertAsync(new PortofolioCryptocurrency
            {
                PortofolioId = portofolioId,
                Amount = amount,
                Symbol = cryptocurrencySymbol,
                InsertedAt = insertedAt ?? TimeProvider.UtcNow
            });

            await SaveAsync();
        }

        private async Task AddPortofolioIfMissingAsync(string userId)
        {
            var query = Query<Portofolio>().Where(x => x.UserId == userId);
            if (await query.AnyAsync())
            {
                return;
            }

            var entity = new Portofolio(userId);
            await InsertAsync(entity);
            await SaveAsync();
        }

        private async Task<T> TryGetCacheAsync<T>(string key, Func<Task<T>> getter)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                return value;
            }

            value = await getter();
            _cache.Set(key, value, TimeSpan.FromHours(1));
            return value;
        }
    }
}