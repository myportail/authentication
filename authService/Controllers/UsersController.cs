using System;
using System.Linq;
using System.Threading.Tasks;
using authService.Models.Api;
using authService.Models.Api.Parameters;
using authService.Models.Api.Results;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
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
