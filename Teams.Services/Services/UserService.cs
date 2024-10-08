using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.DTOs;
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

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<int> AddUser(User user)
        {
            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = DateTime.UtcNow,

            };

            return await _userRepository.AddUser(newUser);
        }

        public async Task<int> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }

        public async Task<bool> CheckEmailExists(CheckEmailRequestDto emailDto)
        {
            string email = emailDto.email;
            return await _userRepository.CheckEmailExists(email);
        }

        public async Task<bool> CheckUsernameExists(CheckUsernameRequestDto usernameDto)
        {
            string username = usernameDto.username;
            return await _userRepository.CheckUsernameExists(username);
        }



    }
}
