using Cryptonite.Core.Enums;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Cryptonite.Infrastructure.Queries.Portofolio.Distribution;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.Today;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos
{
    public class DashboardUpdateEvent : IClientEvent
    {
        public DashboardUpdateEvent(TodayResultResponse todayResult, DistributionQueryResult distribution, AllTimeResultQueryResult allTimeResult)
        {
            TodayResult = todayResult;
            Distribution = distribution;
            AllTimeResult = allTimeResult;
        }

        public TodayResultResponse TodayResult { get; set; }
        public DistributionQueryResult Distribution { get; set; }
        public AllTimeResultQueryResult AllTimeResult { get; set; }
        public ClientEventDestinations Destination => ClientEventDestinations.Dashboard;
    }
}