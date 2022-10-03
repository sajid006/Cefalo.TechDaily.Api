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

namespace Cefalo.TechDaily.Api.Controllers
{
    [Route("api/stories")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly IUriService _uriService;
        public StoryController(IStoryService storyService, IUriService uriService)
        {
            _storyService = storyService;
            _uriService = uriService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetStories([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetStories(validFilter.PageNumber,validFilter.PageSize);
            var totalRecords = await _storyService.CountStories();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Story>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
            //return Ok(await _storyService.GetStories(1, 1));
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
        public async Task<ActionResult<IEnumerable<Story>>> GetSearchedStories([FromQuery] PaginationFilter filter, string pattern)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _storyService.GetSearchedStories(validFilter.PageNumber, validFilter.PageSize, pattern);
            var totalRecords = await _storyService.CountSearchedStories(pattern);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Story>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }
        [HttpGet("users/{username}")]
        public async Task<ActionResult<IEnumerable<Story>>> GetStoriesOfAUser(string username)
        {
            var stories = await _storyService.GetStoriesOfAUser(username);
            return Ok(stories);
        }
    }
}
