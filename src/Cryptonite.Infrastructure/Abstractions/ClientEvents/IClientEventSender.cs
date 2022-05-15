using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.ClientEvents
{
    public interface IClientEventSender
    {
        Task SendToUserAsync(IClientEvent clientEvent, string userId);
    }
}