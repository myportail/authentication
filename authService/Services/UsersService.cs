using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authlib.Services;
using authService.Contexts;
using authService.Model.Api;
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

        public async Task<Model.Business.User> AddUser(Model.Business.User user)
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

                var addedUser = AutoMapper.Map<Model.Business.User>(dbUser);
                
                return addedUser;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<Model.Business.User> GetUserByName(string name)
        {
            try
            {
                var res = UserContext.Users.Where(x => x.Name.Equals(name));
                if (res.Any())
                {
                    return AutoMapper.Map<Model.Business.User>(res.First());
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<Model.Business.User>> listUsers()
        {
            try
            {
                var dbUsers = UserContext.Users.AsEnumerable();

                var usersList = dbUsers
                    .Select(x => AutoMapper.Map<Model.Business.User>(x))
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
