using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Constants;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Commands.TradeEntries.Insert;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cryptonite.UnitTests.Commands
{
    public class InsertTradeEntryCommandHandlerTests
    {
        private static async Task<(InsertTradeEntryCommandHandler handler, IRepository repository)> CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository();
            var portofolioRepository = ServiceHelpers.CreateRepository<IPortofolioRepository, PortofolioRepository>();
            await portofolioRepository.IncreaseCryptocurrencyAmount(TestConstants.UserId, CryptoniteConstants.BaseCryptoQuote, 200);

            return (new InsertTradeEntryCommandHandler(repository, portofolioRepository), repository);
        }

        private InsertTradeEntryCommand BuildCommand()
        {
            return new InsertTradeEntryCommand
            {
                UserId = TestConstants.UserId,
                PaidAmount = 200,
                TradedAt = TimeProvider.UtcNow.Date,
                GainedAmount = 225.241m,
                PaidCryptocurrency = CryptoniteConstants.BaseCryptoQuote,
                GainedCryptocurrency = "ADA"
            };
        }

        [Fact]
        public async Task Inserts_new_buy_entry()
        {
            var (handler, repository) = await CreateSut();
            var command = BuildCommand();
            await handler.Handle(command, new CancellationToken());

            var actual = await repository.Query<TradeEntry>()
                .FirstOrDefaultAsync(x => x.UserId == TestConstants.UserId);

            actual.Should().BeEquivalentTo(new TradeEntry
            {
                UserId = command.UserId,
                PaidAmount = command.PaidAmount,
                TradedAt = command.TradedAt,
                GainedAmount = command.GainedAmount,
                PaidCryptocurrency = command.PaidCryptocurrency,
                GainedCryptocurrency = command.GainedCryptocurrency
            }, options => options.Excluding(x => x.Id));
        }
    }
}