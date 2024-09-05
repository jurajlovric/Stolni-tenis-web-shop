using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TableTennis.Service.Common;
using TableTennis.Model;
using DTO.UserModel;

namespace TableTennis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] RegisterPost registerPost)
        {
            try
            {
                var user = new User
                {
                    Username = registerPost.Username,
                    Email = registerPost.Email,
                    Password = registerPost.Password,
                    RoleId = registerPost.RoleId
                };

                var registeredUser = await _userService.RegisterUserAsync(user);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginPost loginPost)
        {
            try
            {
                var user = await _userService.LoginAsync(loginPost.Email, loginPost.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
