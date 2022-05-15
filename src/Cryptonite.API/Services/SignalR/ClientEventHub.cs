using System;
using System.Threading.Tasks;
using Cryptonite.API.Asp.UserInfo;
using Cryptonite.Infrastructure.Abstractions.ClientEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Cryptonite.API.Services.SignalR
{
    [Authorize]
    public class ClientEventHub : Hub
    {
        private readonly ISignalRConnectionManager _signalRConnectionManager;
        private readonly IUserInfo _userInfo;

        public ClientEventHub(ISignalRConnectionManager signalRConnectionManager, IUserInfo userInfo)
        {
            _signalRConnectionManager = signalRConnectionManager;
            _userInfo = userInfo;
        }

        public string RegisterConnection()
        {
            _signalRConnectionManager.AddConnection(_userInfo.Id, Context.ConnectionId, "");
            return Context.ConnectionId;
        }

        //Called when a connection with the hub is terminated.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _signalRConnectionManager.RemoveConnection(connectionId);
            await Task.FromResult(0);
        }

        public async Task ChangeConnectionPageRoute(string pageRoute)
        {
            _signalRConnectionManager.ChangeConnectionPageRoute(Context.ConnectionId, pageRoute);
        }
    }
}