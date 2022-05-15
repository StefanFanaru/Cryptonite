using System;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Cryptonite.Infrastructure.Services.UserSettingsService
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository _repository;

        public UserSettingsService(IRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<string> GetPreferredCurrency(string userId)
        {
            if (_cache.TryGetValue($"{CacheKeys.PreferredCurrency}_{userId}", out string currency))
            {
                return currency;
            }

            var currencyInDb = await _repository.Query<UserSettings>().Where(x => x.UserId == userId).Select(x => x.PreferredCurrency)
                .FirstOrDefaultAsync() ?? "USD";

            _cache.Set($"{CacheKeys.PreferredCurrency}_{userId}", currencyInDb, TimeSpan.FromHours(1));
            return currencyInDb;
        }

        public async Task<UserSettings> GetUserSettings(string userId)
        {
            return await _repository.Query<UserSettings>().FirstAsync(x => x.UserId == userId);
        }
    }
}