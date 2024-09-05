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

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] RegisterPost registerPost)
        {
            try
            {
                // Mapiranje DTO-a u model User
                var user = new User
                {
                    Username = registerPost.Username,
                    Email = registerPost.Email,
                    Password = registerPost.Password,
                    RoleId = registerPost.RoleId // RoleId se može postaviti prema potrebi
                };

                // Registracija korisnika putem servisa
                var registeredUser = await _userService.RegisterUserAsync(user);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginPost loginPost)
        {
            try
            {
                // Prijava korisnika putem servisa
                var user = await _userService.LoginAsync(loginPost.Email, loginPost.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Prikaz greške ako prijava nije uspješna
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
