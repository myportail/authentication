using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace authService.Services
{
    public interface IUsersService
    {
        Task<AuthLib.Db.Models.User> AddUser(Model.Api.User user);
        Task<List<Model.Api.User>> listUsers();
        Task<AuthLib.Db.Models.User> GetUserByName(string name);
    }
}
