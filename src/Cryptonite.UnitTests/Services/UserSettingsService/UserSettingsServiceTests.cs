using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Cryptonite.UnitTests.Services.UserSettingsService
{
    public class UserSettingsServiceTests
    {
        private IRepository _repository;

        private Infrastructure.Services.UserSettingsService.UserSettingsService CreateSut()
        {
            _repository = ServiceHelpers.CreateRepository();
            return new Infrastructure.Services.UserSettingsService.UserSettingsService(_repository, ServiceHelpers.CreateMemoryCache());
        }

        [Fact]
        public async Task Retrieves_preferred_currency_from_db()
        {
            const string expected = "RON";
            var sut = CreateSut();
            await _repository.InsertAsync(new UserSettings
            {
                UserId = TestConstants.UserId,
                PreferredCurrency = expected,
                BankAccountCurrency = "EUR",
                BankConversionMargin = 1.5m
            });
            await _repository.SaveAsync();

            var actual = await sut.GetPreferredCurrency(TestConstants.UserId);

            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Returns_usd_if_no_preferred_currency()
        {
            var sut = CreateSut();
            var actual = await sut.GetPreferredCurrency(TestConstants.UserId);
            actual.Should().Be("USD");
        }
    }
}