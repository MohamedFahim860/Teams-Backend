using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.DTOs;
using Teams.Domain.Models;

namespace Teams.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(LoginDto loginCred);
    }
}
