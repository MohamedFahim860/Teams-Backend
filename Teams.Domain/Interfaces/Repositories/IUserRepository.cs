using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;

namespace Teams.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetUsers();
        Task<int> GetUserById(int id);
        Task<int> AddUser(User user);
        Task<int> DeleteUser(int id);

    }
}
