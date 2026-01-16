using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpApi.Context;
using CSharpApi.Models;
using CSharpApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CSharpApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(DatabaseContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly DatabaseContext _context = context;

        [HttpPost("register")]
        public ActionResult<User> Register(CreateUserDto userDto)
        {
            var hashedPassword = new PasswordHasher<CreateUserDto>().HashPassword(userDto, userDto.Password);

            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { newUser.Id, newUser.Name, newUser.Email });
        }

        [HttpPost("login")]
        public ActionResult<User> Login(LoginDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Email ou senha inválidos");
            }

            var passwordVerificationResult = new PasswordHasher<LoginDto>().VerifyHashedPassword(loginDto, user.Password, loginDto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Email ou senha inválidos");
            }

            var token = GenerateToken(user);

            return Ok(new { user.Id, user.Name, user.Email, Token = token });
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}