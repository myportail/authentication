using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authlib.Services;
using authService.Contexts;
using authService.Models.Business;
using AutoMapper;

namespace authService.Services
{
    public class UsersService : IUsersService
    {
        private IPasswordHasher PasswordHasher { get; }
        
        private UserContext UserContext { get; }
        private IMapper AutoMapper { get; }
        
        public UsersService(
            IPasswordHasher passwordHasher,
            UserContext userContext,
            IMapper autoMapper)
        {
            UserContext = userContext;
            PasswordHasher = passwordHasher;
            AutoMapper = autoMapper;
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                var dbUser = AutoMapper.Map<Authlib.Models.Db.User>(user);
                dbUser.Password = PasswordHasher.HashPassword(user.Password);
                dbUser.Id = Guid.NewGuid().ToString();
                
                // var dbUser = new Authlib.Models.Db.User()
                // {
                //     Name = user.Name,
                //     Password = PasswordHasher.HashPassword(user.Password),
                //     Id = Guid.NewGuid().ToString()
                // };

                await UserContext.Users.AddAsync(dbUser);
                await UserContext.SaveChangesAsync();

                var addedUser = AutoMapper.Map<User>(dbUser);
                
                return addedUser;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<User> GetUserByName(string name)
        {
            try
            {
                var res = UserContext.Users.Where(x => x.Name.Equals(name));
                if (res.Any())
                {
                    return AutoMapper.Map<User>(res.First());
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
                var dbUsers = UserContext.Users.AsEnumerable();

                var usersList = dbUsers
                    .Select(x => AutoMapper.Map<User>(x))
                    .ToList();
                
                return usersList;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
