using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HubService.API.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        public override async Task OnConnectedAsync()
        {
            string username = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(username))
            {
                _userConnections[username] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            string username = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(username))
            {
                _userConnections.TryRemove(username, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public static bool IsUserOnline(string username)
        {
            return _userConnections.ContainsKey(username);
        }
    }
}
