using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authlib.Services;
using authService.Contexts;
using authService.Model.Api;

namespace authService.Services
{
    public class UsersService : IUsersService
    {
        private IPasswordHasher PasswordHasher { get; }
        
        private UserContext UserContext { get; }
        
        public UsersService(
            IPasswordHasher passwordHasher,
            UserContext userContext)
        {
            UserContext = userContext;
            PasswordHasher = passwordHasher;
        }

        public async Task<AuthLib.Db.Models.User> AddUser(Model.Api.User user)
        {
            try
            {
                var dbUser = new AuthLib.Db.Models.User()
                {
                    Name = user.Name,
                    Password = PasswordHasher.HashPassword(user.Password),
                    Id = Guid.NewGuid().ToString()
                };

                await UserContext.Users.AddAsync(dbUser);
                await UserContext.SaveChangesAsync();
                                
                return dbUser;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<AuthLib.Db.Models.User> GetUserByName(string name)
        {
            try
            {
                var res = UserContext.Users.Where(x => x.Name.Equals(name));
                if (res.Any())
                {
                    return res.First();
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<User>> listUsers()
        {
            try
            {
                var users = new List<User>();
            
                return users;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
