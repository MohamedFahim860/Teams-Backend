using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.DTOs;
using Teams.Domain.Interfaces.Repositories;
using Teams.Domain.Models;
using Teams.Persistence.Context;

namespace Teams.Persistence.Repositories
{
    public class MessageRepository:IMessageRepository
    {
        private readonly TeamsDbContext _context;

        public MessageRepository(TeamsDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddMessage(Message message)
        {
            _context.Message.Add(message);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMessage(int messageId)
        {
            var message = await _context.Message.FindAsync(messageId);

            if (message == null)
            {
                return false;
            }

            _context.Message.Remove(message);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Message> UpdateMessage(int messageId, UpdateMessageDto updateMessage)
        {
            var message = await _context.Message.FindAsync(messageId);
            message.MessageText = updateMessage.MessageText;

            if (message == null)
            {
                return null;
            }

            _context.Message.Update(message);
            var result = await _context.SaveChangesAsync();

            return message;
        }
    }
}
