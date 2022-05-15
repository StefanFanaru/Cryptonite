using System.Collections.Generic;

namespace Cryptonite.Infrastructure.Abstractions.ClientEvents
{
    public interface ISignalRConnectionManager
    {
        void AddConnection(string userId, string connectionId, string pageRoute);
        void RemoveConnection(string connectionId);
        Dictionary<string, string> GetUserConnections(string userId);
        bool IsUserConnected(string userId);
        List<string> GetAllConnectedUsers();
        bool IsConnectedToPage(string userId, string pageRoute);
        void ChangeConnectionPageRoute(string connectionId, string pageRoute);
    }
}