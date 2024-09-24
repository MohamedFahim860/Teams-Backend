using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Interfaces.Repositories;
using Teams.Domain.Interfaces.Services;
using Teams.Domain.Models;

namespace Teams.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<int> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<int> AddUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }



    }
}
