using Cryptonite.Core.Enums;

namespace Cryptonite.Infrastructure.Abstractions.ClientEvents
{
    public interface IClientEvent
    {
        public ClientEventDestinations Destination { get; }
    }
}