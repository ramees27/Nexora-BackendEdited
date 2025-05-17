using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace Common.SignalRHub
{
    // Hubs/VideoHub.cs


    public class VideoHub : Hub
    {
        public async Task SendOffer(string toConnectionId, string offer)
        {
            await Clients.Client(toConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string toConnectionId, string answer)
        {
            await Clients.Client(toConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendIceCandidate(string toConnectionId, string candidate)
        {
            await Clients.Client(toConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }


}
