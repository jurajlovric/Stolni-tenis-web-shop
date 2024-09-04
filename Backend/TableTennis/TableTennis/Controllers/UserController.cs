using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TableTennis.Service.Common;
using DTO.UserModel;
using TableTennis.Model;

namespace TableTennis.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenData>> RegisterUser([FromBody] RegisterPost registerPost)
        {
            try
            {
                // Provjerite da su svi potrebni podaci prisutni
                if (string.IsNullOrEmpty(registerPost.Password))
                {
                    return BadRequest(new { message = "Lozinka ne smije biti prazna." });
                }

                // Mapiranje RegisterPost DTO-a u User model
                var user = _mapper.Map<User>(registerPost);

                // Registracija korisnika putem servisa
                var registeredUser = await _userService.RegisterUserAsync(user);

                // Mapiranje User modela u TokenData DTO za povrat klijentu
                var tokenData = _mapper.Map<TokenData>(registeredUser);

                return Ok(tokenData);
            }
            catch (Exception ex)
            {
                // Prikaz greške ako korisnik već postoji ili dođe do greške prilikom registracije
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenData>> LoginUser([FromBody] LoginPost loginPost)
        {
            try
            {
                // Prijava korisnika putem servisa
                var user = await _userService.LoginAsync(loginPost.Email, loginPost.Password);

                // Mapiranje User modela u TokenData DTO za povrat klijentu
                var tokenData = _mapper.Map<TokenData>(user);

                return Ok(tokenData);
            }
            catch (Exception ex)
            {
                // Prikaz greške ako prijava nije uspješna (pogrešan email ili lozinka)
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
