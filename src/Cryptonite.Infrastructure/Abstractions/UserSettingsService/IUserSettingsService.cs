using System.Threading.Tasks;
using Cryptonite.Core.Entities;

namespace Cryptonite.Infrastructure.Abstractions.UserSettingsService
{
    public interface IUserSettingsService
    {
        Task<string> GetPreferredCurrency(string userId);
        Task<UserSettings> GetUserSettings(string userId);
    }
}