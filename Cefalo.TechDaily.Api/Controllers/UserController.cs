using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Cefalo.TechDaily.Service.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Cefalo.TechDaily.Api.Wrappers;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto?>>> GetUsersAsync()
        {
            return Ok(await _userService.GetUsersAsync());
        }
        [HttpGet("{Username}")]
        public async Task<IActionResult> GetUserByUsernameAsync(string Username)
        {
            var user = await _userService.GetUserByUsernameAsync(Username);
            if (user == null) return BadRequest("User not found");
            return Ok(user);
            //return Ok(new Response<UserDto>(user));
        }
        [HttpPost]
        public async Task<IActionResult> PostUserAsync(SignupDto request)
        {
            var userWithToken = await _userService.PostUserAsync(request);
            if (userWithToken == null) return BadRequest("Can't create user");
            return Created(nameof(PostUserAsync), userWithToken);
        }
        [HttpPatch("{Username}"), Authorize]
        public async Task<IActionResult> UpdateUserAsync(string Username, UpdateUserDto updateUserDto)
        {
            var userDto = await _userService.UpdateUserAsync(Username,updateUserDto);
            if (userDto == null) return BadRequest("Can't update user");
            return Ok(userDto);
        }
        [HttpDelete("{Username}"), Authorize]
        public async Task<IActionResult> DeleteUserAsync(string Username)
        {
            var deleted = await _userService.DeleteUserAsync(Username);
            if(!deleted) return BadRequest("Can't delete user");
            return NoContent();
        }
        
    }
}

