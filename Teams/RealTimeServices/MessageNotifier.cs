using Microsoft.AspNetCore.SignalR;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Services;
using Teams.Hubs;

namespace Teams.RealTimeServices
{
    public class MessageNotifier
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageNotifier(IMessageService messageSerivice, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageSerivice;
            _hubContext = hubContext;
        }

        public async Task<int> AddMessageAndNotify(SendMessageDto message)
        {
            var messageId = await _messageService.AddMessage(message);

            // Broadcast the message to SignalR clients
            await _hubContext.Clients.Group($"Channel-{message.message.ChannelId}")
                .SendAsync("ReceiveMessage", new
                {
                    MessageText = message.message.MessageText,
                    SenderId = message.message.UserId,
                    SentAt = DateTime.UtcNow
                });

            return messageId;

        }
    }
}
