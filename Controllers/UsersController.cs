using CSharpApi.Constants;
using CSharpApi.Models;
using CSharpApi.Models.DTOs;
using CSharpApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(UserService userService) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<User>>> Index()
        {
            var users = await userService.GetAllUsers();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await userService.GetUserById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }


        [Authorize]
        [HttpGet("name/{name}")]
        public async Task<ActionResult<List<User>>> GetByName(string name)
        {
            var users = await userService.GetUsersByName(name);

            return Ok(users);
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto user)
        {
            var newUser = await userService.CreateUser(user);

            return Ok(newUser);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserDto updatedUser)
        {
            var user = await userService.UpdateUser(id, updatedUser);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await userService.DeleteUser(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}