using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Serilog;

namespace Cryptonite.API.Services.SignalR
{
    public class SignalRConnectionManager : ISignalRConnectionManager
    {
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> ConnectionMap = new();
        private readonly IBinanceSocket _binanceSocket;

        public SignalRConnectionManager(IBinanceSocket binanceSocket)
        {
            _binanceSocket = binanceSocket;
        }

        public void AddConnection(string userId, string connectionId, string pageRoute)
        {
            var connections = GetUserConnections(userId);

            lock (connections)
            {
                if (HasConnection(userId, connectionId))
                {
                    ChangeConnectionPageRoute(connectionId, pageRoute);
                    return;
                }

                connections.Add(connectionId, pageRoute);
            }

            Log.Debug($"User {userId} has established a connection with the client");
        }

        public bool IsConnectedToPage(string userId, string pageRoute)
        {
            var connections = GetUserConnections(userId);

            lock (connections)
            {
                return connections.Values.Any(pr => pr == pageRoute);
            }
        }

        public void ChangeConnectionPageRoute(string connectionId, string pageRoute)
        {
            var usersConnection = ConnectionMap.FirstOrDefault(x => x.Value.ContainsKey(connectionId));
            usersConnection.Value[connectionId] = pageRoute;

            if (!HasAnyRouteConnections("/dashboard"))
            {
                _binanceSocket.StopMiniTickerConnection();
            }
            else if (!_binanceSocket.IsAlive())
            {
                _binanceSocket.StartMiniTickerConnection();
            }

            Log.Debug($"User with connectionId {connectionId} changed it's page route to {pageRoute}");
            Log.Debug("Connections open: " + ConnectionMap.Count);
        }

        public void RemoveConnection(string connectionId)
        {
            var userConnections = ConnectionMap.FirstOrDefault(x => x.Value.ContainsKey(connectionId));

            if (userConnections.Value == null)
            {
                return;
            }

            lock (userConnections.Value)
            {
                userConnections.Value.Remove(connectionId);
            }

            if (userConnections.Value.Count == 0)
            {
                ConnectionMap.TryRemove(userConnections);
            }

            if (!HasAnyRouteConnections("/dashboard"))
            {
                _binanceSocket.StopMiniTickerConnection();
            }

            Log.Debug($"Removing connection {connectionId}");
        }

        public Dictionary<string, string> GetUserConnections(string userId)
        {
            return ConnectionMap.GetOrAdd(userId, s => new Dictionary<string, string>());
        }

        public List<string> GetAllConnectedUsers()
        {
            return ConnectionMap.Select(x => x.Key).ToList();
        }

        public bool IsUserConnected(string userId)
        {
            return ConnectionMap.Any(x => x.Key == userId);
        }

        private bool HasAnyRouteConnections(string pageRoute)
        {
            return ConnectionMap.Any(x => x.Value.Any(c => c.Value == pageRoute));
        }

        private bool HasConnection(string userId, string connectionId)
        {
            return ConnectionMap.ContainsKey(userId) && ConnectionMap[userId].ContainsKey(connectionId);
        }
    }
}