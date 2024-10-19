using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;

namespace Teams.Domain.Interfaces.Repositories
{
    public interface IChannelRepository
    {
        Task<Channel?> GetDirectChatChannel(int senderId, int receiverId);
        Task<Channel> CreateDirectChatChannel(int senderId, int receiverId);
    }
}
