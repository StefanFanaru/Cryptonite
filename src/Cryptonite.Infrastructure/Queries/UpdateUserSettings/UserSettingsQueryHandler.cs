using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Queries.UpdateUserSettings
{
    public class UserSettingsQueryHandler : IRequestHandler<UserSettingsQuery, IOperationResult<UserSettingsQueryResponse>>
    {
        private readonly IRepository _repository;

        public UserSettingsQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IOperationResult<UserSettingsQueryResponse>> Handle(UserSettingsQuery request,
            CancellationToken cancellationToken)
        {
            var settings = await _repository.Query<UserSettings>()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            return ResultBuilder.Ok(new UserSettingsQueryResponse
            {
                PreferredCurrency = settings?.PreferredCurrency,
                BankAccountCurrency = settings?.BankAccountCurrency,
                BankConversionMargin = settings?.BankConversionMargin ?? 0m
            });
        }
    }
}