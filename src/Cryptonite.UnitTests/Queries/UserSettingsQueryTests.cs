using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Queries.UpdateUserSettings;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Cryptonite.UnitTests.Queries
{
    public class UserSettingsQueryTests
    {
        public (UserSettingsQueryHandler handler, IRepository repository) CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository();

            return (new UserSettingsQueryHandler(repository), repository);
        }

        [Fact]
        public async Task Returns_user_settings()
        {
            var (handler, repository) = CreateSut();
            var expected = new UserSettings
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = "USD",
                BankConversionMargin = 1.1m
            };
            await repository.InsertAsync(expected);
            await repository.SaveAsync();


            var actual = await handler.Handle(
                new UserSettingsQuery().WithUserId(TestConstants.UserId), new CancellationToken());

            actual.Result.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.UserId));
        }
    }
}