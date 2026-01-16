using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpApi.Context;
using CSharpApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(DatabaseContext context) : ControllerBase
    {

        private readonly DatabaseContext _context = context;

        [HttpGet]
        public ActionResult<List<User>> Index()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return Ok(user);
        }



        [HttpGet("name/{name}")]
        public ActionResult<List<User>> GetByName(string name)
        {
            var users = _context.Users
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
                .ToList();

            return Ok(users);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}