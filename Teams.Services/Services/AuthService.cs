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
    public class AuthService : IAuthService
    {
        private readonly TeamsDbContext _context;

        public AuthService(TeamsDbContext context)
        {
            _context = context;
        }
        public async Task<User> AuthenticateUser(LoginDto loginCred)
        {
            // Find the user in the database by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginCred.Username);

            // If the user does not exist, return Unauthorized
            if (user == null)
            {
                return null;
            }

            if (user.PasswordHash != loginCred.Password)
            {
                return null;
            }

            return user;

        }
    }
}
