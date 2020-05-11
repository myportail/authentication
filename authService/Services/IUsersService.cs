using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authlib.Models.Db;
using User = authService.Models.Business.User;

namespace authService.Services
{
    public interface IUsersService
    {
        Task<User> AddUser(User user);
        Task<List<User>> listUsers();
        Task<User> GetUserByName(string name);
    }
}
