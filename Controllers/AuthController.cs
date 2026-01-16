using CSharpApi.Models.DTOs;
using CSharpApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(CreateUserDto userDto)
        {
            var newUser = await authService.RegisterAsync(userDto);
            if (newUser == null)
            {
                return BadRequest("User registration failed.");
            }

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var token = await authService.LoginAsync(loginDto);
            if (token is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(token);
        }
    }
}