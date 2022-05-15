using System;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.Portofolio.Distribution;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.Today;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets
{
    public class DashboardUpdater : INotificationHandler<MiniTickersReceived>
    {
        private readonly IClientEventSender _clientEventSender;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISignalRConnectionManager _signalRConnectionManager;

        public DashboardUpdater(ISignalRConnectionManager signalRConnectionManager, IClientEventSender clientEventSender,
            IServiceScopeFactory serviceScopeFactory)
        {
            _signalRConnectionManager = signalRConnectionManager;
            _clientEventSender = clientEventSender;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Handle(MiniTickersReceived notification, CancellationToken cancellationToken)
        {
            var connectedUsers = _signalRConnectionManager.GetAllConnectedUsers();

            foreach (var userId in connectedUsers)
            {
                if (!_signalRConnectionManager.IsConnectedToPage(userId, "/dashboard"))
                {
                    continue;
                }

                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var todayResult = await mediator.Send(new TodayResultQuery().WithUserId(userId), cancellationToken);
                    var distribution = await mediator.Send(new DistributionQuery().WithUserId(userId), cancellationToken);
                    var allTimeResult = await mediator.Send(new AllTimeResultQuery().WithUserId(userId), cancellationToken);

                    var clientEvent = new DashboardUpdateEvent(todayResult.Result, distribution.Result, allTimeResult.Result);
                    await _clientEventSender.SendToUserAsync(clientEvent, userId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}