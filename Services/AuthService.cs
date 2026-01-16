using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpApi.Context;
using CSharpApi.Models;
using CSharpApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CSharpApi.Services
{
    public class AuthService(DatabaseContext context, UserService userService, IConfiguration configuration)
    {
        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            var user = context.Users.SingleOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return null;
            }

            var passwordValid = new PasswordHasher<LoginDto>().VerifyHashedPassword(loginDto, user.Password, loginDto.Password);
            if (passwordValid == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return GenerateToken(user);
        }

        public async Task<UserResponseDto> RegisterAsync(CreateUserDto userDto)
        {
            var userResponse = await userService.CreateUser(userDto);

            return userResponse;
        }

        string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
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