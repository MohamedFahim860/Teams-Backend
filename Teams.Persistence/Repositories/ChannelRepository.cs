using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Interfaces.Repositories;
using Teams.Domain.Models;
using Teams.Persistence.Context;

namespace Teams.Persistence.Repositories
{
    public class ChannelRepository: IChannelRepository
    {
        private readonly TeamsDbContext _context;

        public ChannelRepository(TeamsDbContext context)
        {
            _context = context;
        }

        public async Task<Channel?> GetDirectChatChannel(int senderId, int receiverId)
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

        public async Task<Channel> CreateDirectChatChannel(int senderId, int receiverId)
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

    }
}
