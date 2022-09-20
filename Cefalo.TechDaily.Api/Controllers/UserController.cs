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
            return Ok(await _userService.GetUsers());
        }
        [HttpGet("{Username}")]
        public async Task<IActionResult> GetUserByUsername(string Username)
        {
            var user = await _userService.GetUserByUsername(Username);
            if (user == null) return BadRequest("User not found");
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> PostUser(SignupDto request)
        {
            var userDto = await _userService.PostUser(request);
            if (userDto == null) return BadRequest("Cant create user");
            return CreatedAtAction(nameof(PostUser),userDto);
        }
        [HttpPatch("{Username}")]
        public async Task<IActionResult> UpdateUser(string Username, UpdateUserDto updateUserDto)
        {
            //if (Username != updateUserDto.Username) return BadRequest("Username does not match");
            //System.Diagnostics.Debug.WriteLine("hello");
            var userDto = await _userService.UpdateUser(Username,updateUserDto);
            if (userDto == null) return BadRequest("User not found");
            return Ok(userDto);
        }
        [HttpDelete("{Username}")]
        public async Task<IActionResult> DeleteUser(string Username)
        {
            var deleted = await _userService.DeleteUser(Username);
            if(!deleted) return BadRequest("User not found");
            return NoContent();
        }
    }
}

