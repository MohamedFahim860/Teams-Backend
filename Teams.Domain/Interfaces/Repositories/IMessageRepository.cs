using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.DTOs;
using Teams.Domain.Models;

namespace Teams.Domain.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<int> AddMessage(Message message);
        Task<bool> DeleteMessage(int messageId);
        Task<Message> UpdateMessage(int messageId, UpdateMessageDto updateMessage);
    }
}
