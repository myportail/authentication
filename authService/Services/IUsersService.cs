using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authlib.Models.Db;

namespace authService.Services
{
    public interface IUsersService
    {
        Task<Model.Business.User> AddUser(Model.Business.User user);
        Task<List<Model.Business.User>> listUsers();
        Task<Model.Business.User> GetUserByName(string name);
    }
}
