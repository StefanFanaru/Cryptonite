using System;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Cryptonite.Infrastructure.Commands.UpdateUserSettings
{
    public class UpdateUserSettingsCommandHandler : IRequestHandler<UpdateUserSettingsCommand, IOperationResult<Unit>>
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository _repository;

        public UpdateUserSettingsCommandHandler(IRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IOperationResult<Unit>> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var query = _repository.Query<UserSettings>().Where(x => x.UserId == request.UserId);

            if (await query.AnyAsync(cancellationToken))
            {
                await query.UpdateFromQueryAsync(x => new UserSettings
                {
                    PreferredCurrency = request.PreferredCurrency,
                    BankAccountCurrency = request.BankAccountCurrency,
                    BankConversionMargin = request.BankConversionMargin
                }, cancellationToken);

                return ResultBuilder.Ok();
            }

            await _repository.InsertAsync(new UserSettings
            {
                UserId = request.UserId,
                PreferredCurrency = request.PreferredCurrency,
                BankAccountCurrency = request.BankAccountCurrency,
                BankConversionMargin = request.BankConversionMargin
            });

            await _repository.SaveAsync();
            _cache.Set($"{CacheKeys.PreferredCurrency}_{request.UserId}", request.PreferredCurrency, TimeSpan.FromHours(1));

            return ResultBuilder.Ok();
        }
    }
}