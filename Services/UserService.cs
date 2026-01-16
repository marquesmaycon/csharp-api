using CSharpApi.Context;
using CSharpApi.Models.DTOs;
using CSharpApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CSharpApi.Services
{
    public class UserService(DatabaseContext context, CurrentUserService currentUserService)
    {

        public async Task<List<User>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsersByName(string name)
        {
            return await context.Users
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<UserResponseDto> CreateUser(CreateUserDto userDto)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email.ToLower());

            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var hashedPassword = new PasswordHasher<CreateUserDto>().HashPassword(userDto, userDto.Password);
            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword,
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };
        }

        public async Task<User?> UpdateUser(int id, UpdateUserDto updatedUser)
        {
            var isAdmin = currentUserService.IsAdmin;
            var loggedUserId = currentUserService.UserId;

            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            if (!isAdmin && user.Id != loggedUserId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para atualizar este usuário.");
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.Role))
            {
                if (!isAdmin && user.Role != updatedUser.Role)
                {
                    throw new UnauthorizedAccessException("Apenas administradores podem alterar roles.");
                }
                user.Role = updatedUser.Role;
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var isAdmin = currentUserService.IsAdmin;
            var loggedUserId = currentUserService.UserId;

            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            if (!isAdmin && user.Id != loggedUserId)
            {
                throw new UnauthorizedAccessException("Você não tem permissão para deletar este usuário.");
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return true;
        }
    }
}