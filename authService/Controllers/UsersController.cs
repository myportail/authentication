using System;
using System.Linq;
using System.Threading.Tasks;
using authService.Models.Api;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authService.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private Services.IUsersService UsersService { get; }
        private IMapper AutoMapper { get; }

        public UsersController(
            Services.IUsersService usersService,
            IMapper autoMapper)
        {
            UsersService = usersService;
            AutoMapper = autoMapper;
        }

        [Route("create")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser createUser)
        {
            try
            {
                // test line to query claim data
                var userId = HttpContext.User.Claims.First(x => x.Type == "id")?.Value;

                var user = AutoMapper.Map<Models.Business.User>(createUser);
                
                var dbUser = await UsersService.AddUser(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("")]
        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> ListUsers()
        {
            try
            {
                var users = await UsersService.listUsers();
                var retUsers = users.Select(x => AutoMapper.Map<User>(x));
                return Ok(retUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
