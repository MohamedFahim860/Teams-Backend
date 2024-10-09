using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Repositories;
using Teams.Domain.Interfaces.Services;
using Teams.Domain.Models;
using Teams.Persistence.Context;
using Teams.Persistence.Repositories;

namespace Teams.Services.Services
{
    public class MessageService:IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly TeamsDbContext _context;

        public MessageService(IMessageRepository messageRepository, TeamsDbContext context) 
        {
            _messageRepository = messageRepository;
            _context = context;
        }
        public async Task<int> AddMessage(SendMessageDto message)
        {
            int senderId = message.message.UserId;
            // Step 1: Check if there is already a direct chat channel between sender and receiver
            var channel = await GetDirectChatChannel(senderId, message.ReceiverId);

            if (channel == null)
            {
                // Step 2: If no direct channel exists, create a new direct chat channel
                channel = await CreateDirectChatChannel(senderId, message.ReceiverId);
            }

            // Step 3: Create and save the message
            var messageObj = new Message
            {
                MessageText = message.message.MessageText,
                SentAt = DateTime.UtcNow,
                ChannelId = channel.ChannelId,  // Use the existing or newly created ChannelId
                UserId = senderId
            };

            _context.Message.Add(messageObj);
            await _context.SaveChangesAsync();

            //return the ID of the newly created message
            return messageObj.MessageId;

            //return await _messageRepository.AddMessage(message);
        }

        // Helper function to check if a direct chat channel exists between two users
        private async Task<Channel?> GetDirectChatChannel(int senderId, int receiverId)
        {
            // Search for a common channel in the UserChannel table
            var channelUserEntries = await _context.UserChannels
                .Where(uc => uc.UserId == senderId || uc.UserId == receiverId) // Check for either user
                .GroupBy(uc => uc.ChannelId)
                .Where(g => g.Count() == 2 && g.Any(uc => uc.UserId == senderId) && g.Any(uc => uc.UserId == receiverId)) // Ensure both sender and receiver are present
                .Select(g => g.Key)
                .ToListAsync();

            // Check if any of these channels is a direct chat
            var directChatChannel = await _context.Channels
                .Where(c => channelUserEntries.Contains(c.ChannelId) && c.IsDirectChat == true)
                .FirstOrDefaultAsync();

            return directChatChannel;
        }

        // Helper function to create a new direct chat channel
        private async Task<Channel> CreateDirectChatChannel(int senderId, int receiverId)
        {
            // Create a new channel without a name and set IsDirectChat to true
            var newChannel = new Channel
            {
                IsDirectChat = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Channels.Add(newChannel);
            await _context.SaveChangesAsync(); // Save the new channel to generate ChannelId

            // Add records in UserChannel for both users (sender and receiver)
            var senderUserChannel = new UserChannel
            {
                UserId = senderId,
                ChannelId = newChannel.ChannelId
            };

            var receiverUserChannel = new UserChannel
            {
                UserId = receiverId,
                ChannelId = newChannel.ChannelId
            };

            _context.UserChannels.Add(senderUserChannel);
            _context.UserChannels.Add(receiverUserChannel);

            await _context.SaveChangesAsync(); // Save UserChannel records

            return newChannel;
        }


        // Function to get all messages between two users (direct chat)
        public async Task<List<Message>> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            // Step 1: Get the direct chat channel between the two users
            var channel = await GetDirectChatChannel(userId1, userId2);

            // Step 2: If no channel exists, return an empty list (no messages)
            if (channel == null)
            {
                return new List<Message>();
            }

            // Step 3: Fetch all messages from that channel
            var messages = await _context.Message
                .Where(m => m.ChannelId == channel.ChannelId)
                .OrderBy(m => m.SentAt) // Order by the time they were sent
                .ToListAsync();

            return messages;
        }

        public async Task<bool> DeleteMessage(int messageId)
        {
            return await _messageRepository.DeleteMessage(messageId); 
        }

        public async Task<Message> UpdateMessage(int messageId, UpdateMessageDto updateMessage)
        {
            return await _messageRepository.UpdateMessage(messageId, updateMessage);
        }

    }
}
