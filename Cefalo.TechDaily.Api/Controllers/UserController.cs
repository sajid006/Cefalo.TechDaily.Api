using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
            return Ok(await _context.Users.ToListAsync());
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<List<User>>> GetUser(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null) return BadRequest("User not found");
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), user.Id, GetUser(user.Id));
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(int Id, User request)
        {
            if (Id != request.Id) return BadRequest("Id does not match");
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null) return BadRequest("User not found");
            user.Name = request.Name;
            user.Email = request.Email;
            user.CreatedAt = request.CreatedAt;
            user.UpdatedAt = request.UpdatedAt;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), user.Id, GetUser(user.Id));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null) return BadRequest("User not found");
            _context.Users.Remove((User)user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

