using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/stories")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
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
            var newStory = await _storyService.PostStory(postStoryDto);
            if (newStory == null) return BadRequest("Cant post story");
            return CreatedAtAction(nameof(PostStory), newStory);
        }

        [HttpPatch("{Id}"), Authorize]
        public async Task<IActionResult> UpdateStory(int Id, UpdateStoryDto updateStoryDto)
        {
            var story = await _storyService.UpdateStory(Id, updateStoryDto);
            if (story == null) return BadRequest("Story not found");
            return Ok(story);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteStory(int Id)
        {
            var deleted = await _storyService.DeleteStory(Id);
            if (!deleted) return BadRequest("Story not found");
            return NoContent();
        }
        [HttpGet("search/{pattern}")]
        public async Task<ActionResult<IEnumerable<Story>>> GetSearchedStories(string pattern)
        {
            var stories = await _storyService.GetSearchedStories(pattern);
            return Ok(stories);

        }
        [HttpGet("users/{username}")]
        public async Task<ActionResult<IEnumerable<Story>>> GetStoriesOfAUser(string username)
        {
            var stories = await _storyService.GetStoriesOfAUser(username);
            return Ok(stories);
        }
    }
}
