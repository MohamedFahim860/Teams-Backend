using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Security.Claims;

namespace Teams.Hubs
{
    public class ChatHub : Hub
    {
        // Method to send a message to a specific channel
        public async Task SendMessage(int channelId, string message)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//The User property gives you access to the ClaimsPrincipal representing the user connected to the hub.
                                                                                  //This allows you to retrieve user information, such as user ID or roles.
                                                                                  //The User property is populated based on the authentication mechanism you use in your
                                                                                  //application(e.g., JWT, cookie-based authentication).
            if (userId == null)
            {
                // Handle unauthenticated user
                return;
            }

            // Optionally, you can add message persistence here or use existing services

            // Broadcast the message to all clients in the channel group
            await Clients.Group($"Channel-{channelId}").SendAsync("ReceiveMessage", new
            {
                ChannelId = channelId,
                Message = message,
                SenderId = int.Parse(userId),
                SentAt = DateTime.UtcNow
            });
        }

        // Method for clients to join a channel group
        public async Task JoinChannel(int channelId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Channel-{channelId}");
        }

        // Method for clients to leave a channel group
        public async Task LeaveChannel(int channelId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Channel-{channelId}");
        }

        // Optional: Override OnConnectedAsync and OnDisconnectedAsync for logging or additional logic
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            // Additional logic on connection
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            // Additional logic on disconnection
        }

    }
}
