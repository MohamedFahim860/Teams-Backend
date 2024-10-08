﻿using Microsoft.EntityFrameworkCore;
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
    public class UserRepository : IUserRepository
    {
        private readonly TeamsDbContext _context;

        public UserRepository(TeamsDbContext context) 
        { 
            _context = context;
        }

        public async Task<int> GetUsers()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<int> AddUser(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            var isTaken =await _context.Users.AnyAsync(u => u.Email == email);
            return isTaken;
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            var isTaken = await _context.Users.AnyAsync(u => u.UserName == username);
            return isTaken;
        }



    }
}
