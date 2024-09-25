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
    }
}
