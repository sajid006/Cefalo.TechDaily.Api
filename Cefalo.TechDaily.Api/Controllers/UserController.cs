using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using AutoMapper;

namespace Cefalo.TechDaily.Api.Controllers
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto?>>> Getusers()
        {
            return Ok(await _userService.GetUsersAsync());
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var user = await _userService.GetUserByIdAsync(Id);
            if (user == null) return BadRequest("User not found");
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            var newUserDto = await _userService.PostUser(user);
            return Created("",newUserDto);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(int Id, User request)
        {
            if (Id != request.Id) return BadRequest("Id does not match");
            var userDto = await _userService.UpdateUser(Id,request);
            if (userDto == null) return BadRequest("User not found");
            return Ok(userDto);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var deleted = await _userService.DeleteUser(Id);
            if(!deleted) return BadRequest("User not found");
            return NoContent();
        }
    }
}

