using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly IAuthService _authService;
        public StoryController(IStoryService storyService, IAuthService authService)
        {
            _storyService = storyService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetStories()
        {
            return Ok(await _storyService.GetStories());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStory(int Id)
        {
            var story = await _storyService.GetStoryById(Id);
            if (story == null) return BadRequest("Story not found");
            return Ok(story);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> PostStory(PostStoryDto postStoryDto)
        {
            var loggedInUser = _authService.GetMyName();
            if (loggedInUser != postStoryDto.AuthorName) return BadRequest("You are not authorized to post story");
            var newStory = await _storyService.PostStory(postStoryDto);
            if (newStory == null) return BadRequest("Cant post story");
            return CreatedAtAction(nameof(PostStory), newStory);
        }

        [HttpPatch("{Id}"), Authorize]
        public async Task<IActionResult> UpdateStory(int Id, UpdateStoryDto updateStoryDto)
        {   var loggedInUser = _authService.GetMyName();
            Boolean Auth = await CheckAuthor(loggedInUser, Id);
            if (!Auth)return BadRequest("You are not authorized to update story");
            var story = await _storyService.UpdateStory(Id, updateStoryDto);
            if (story == null) return BadRequest("Story not found");
            return Ok(story);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteStory(int Id)
        {
            var loggedInUser = _authService.GetMyName();
            Boolean Auth = await CheckAuthor(loggedInUser, Id);
            if(!Auth)return BadRequest("You are not authorized to delete story");
            var deleted = await _storyService.DeleteStory(Id);
            if (!deleted) return BadRequest("Story not found");
            return NoContent();
        }
        private async Task<Boolean> CheckAuthor(string loggedInUser, int Id)
        {
            if (loggedInUser == null) return false;
            var curstory = await _storyService.GetStoryById(Id);
            if (curstory == null) return false;
            var authorName = ((Story)curstory).AuthorName;
            if(authorName != loggedInUser) return false;
            return true;
        }
    }
}
