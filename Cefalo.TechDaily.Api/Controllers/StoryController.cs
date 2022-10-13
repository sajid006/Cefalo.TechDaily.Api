using Cefalo.TechDaily.Api.Filter;
using Cefalo.TechDaily.Api.Wrappers;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using Cefalo.TechDaily.Service.Services;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cefalo.TechDaily.Api.Helpers;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

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
        public async Task<ActionResult<IEnumerable<Story>>> GetStoriesAsync([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetStoriesAsync(validFilter.PageNumber,validFilter.PageSize);
            var totalRecords = await _storyService.CountStoriesAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Story>(pagedData, validFilter, totalRecords);
            return Ok(pagedReponse);
            //return Ok(await _storyService.GetStories(1, 1));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStoryAsync(int Id)
        {
            var story = await _storyService.GetStoryByIdAsync(Id);
            if (story == null) return BadRequest("Story not found");
            return Ok(story);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> PostStoryAsync(PostStoryDto postStoryDto)
        {
            var newStory = await _storyService.PostStoryAsync(postStoryDto);
            if (newStory == null) return BadRequest("Cant post story");
            return Ok(newStory);
        }

        [HttpPatch("{Id}"), Authorize]
        public async Task<IActionResult> UpdateStoryAsync(int Id, UpdateStoryDto updateStoryDto)
        {
            var story = await _storyService.UpdateStoryAsync(Id, updateStoryDto);
            if (story == null) return BadRequest("Story not found");
            return Ok(story);
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> DeleteStoryAsync(int Id)
        {
            var deleted = await _storyService.DeleteStoryAsync(Id);
            if (!deleted) return BadRequest("Story not found");
            return NoContent();
        }
        [HttpGet("search/{pattern}")]
        public async Task<ActionResult<IEnumerable<Story>>> GetSearchedStoriesAsync([FromQuery] PaginationFilter filter, string pattern)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetSearchedStoriesAsync(validFilter.PageNumber, validFilter.PageSize, pattern);
            var totalRecords = await _storyService.CountSearchedStoriesAsync(pattern);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Story>(pagedData, validFilter, totalRecords);
            return Ok(pagedReponse);

        }
        [HttpGet("users/{username}")]
        public async Task<ActionResult<IEnumerable<Story>>> GetStoriesOfAUserAsync([FromQuery] PaginationFilter filter, string username)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetStoriesOfAUserAsync(validFilter.PageNumber, validFilter.PageSize, username);
            var totalRecords = await _storyService.CountStoriesOfAUserAsync(username);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Story>(pagedData, validFilter, totalRecords);
            return Ok(pagedReponse);
        }
    }
}
