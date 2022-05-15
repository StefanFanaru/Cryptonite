using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Helpers;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Cryptonite.API.Services.SignalR
{
    public class ClientEventSender : IClientEventSender
    {
        private readonly IHubContext<ClientEventHub> _hubContext;
        private readonly ISignalRConnectionManager _signalRConnection;

        public ClientEventSender(IHubContext<ClientEventHub> hubContext, ISignalRConnectionManager signalRConnection)
        {
            _hubContext = hubContext;
            _signalRConnection = signalRConnection;
        }

        public async Task SendToUserAsync(IClientEvent clientEvent, string userId)
        {
            var connections = _signalRConnection.GetUserConnections(userId);
            await SendToConnectionsAsync(clientEvent, connections.Keys);
        }

        private async Task SendToConnectionsAsync(IClientEvent clientEvent, IEnumerable<string> connections)
        {
            if (connections == null)
            {
                Log.Debug("No connections were found");
                return;
            }

            try
            {
                foreach (var connection in connections)
                {
                    try
                    {
                        var appEvent = new ClientEvent(clientEvent);
                        await _hubContext.Clients.Clients(connection).SendAsync("client-events", appEvent.ToJson());
                        Log.Debug($"Sending ClientEvent to connection {connection}");
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }
        }
    }
}