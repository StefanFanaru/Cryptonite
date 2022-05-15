using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Commands.UpdateUserSettings;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cryptonite.UnitTests.Commands
{
    public class UpdateUserSettingsTests
    {
        private (UpdateUserSettingsCommandHandler handler, IRepository repository) CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository();
            return (new UpdateUserSettingsCommandHandler(repository, ServiceHelpers.CreateMemoryCache()), repository);
        }

        [Fact]
        public async Task Inserts_user_settings_when_not_existent()
        {
            var (handler, repository) = CreateSut();
            await handler.Handle(new UpdateUserSettingsCommand
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 1.1m
            }, new CancellationToken());


            var actual = await repository.Query<UserSettings>()
                .FirstOrDefaultAsync(x => x.UserId == TestConstants.UserId);

            actual.Should().BeEquivalentTo(new UserSettings
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 1.1m
            });
        }

        [Fact]
        public async Task Updates_user_settings_when_existent()
        {
            var (handler, repository) = CreateSut();
            await repository.InsertAsync(new UserSettings
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 1.1m
            });
            await repository.SaveAsync();
            repository.ClearChangeTracker();

            await handler.Handle(new UpdateUserSettingsCommand
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 2m
            }, new CancellationToken());


            var query = repository.Query<UserSettings>().Where(x => x.UserId == TestConstants.UserId);
            var actual = await query.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(new UserSettings
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 2m
            });

            (await query.ToListAsync()).Should().HaveCount(1);
        }
    }
}