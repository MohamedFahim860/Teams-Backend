using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;

namespace Teams.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<int> GetUsers();
        Task<int> GetUserById(int Id);
        Task<int> AddUser(User user);
        Task<int> DeleteUser(int id);
    }
}
