
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Cefalo.TechDaily.Service.Contracts;
using Cefalo.TechDaily.Service.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cefalo.TechDaily.Service.Utils.Contracts;
using Cefalo.TechDaily.Service.CustomExceptions;
using Cefalo.TechDaily.Service.DtoValidators;

namespace Cefalo.TechDaily.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly BaseDtoValidator<PostStoryDto> _postStoryDtoValidator;
        private readonly BaseDtoValidator<UpdateStoryDto> _updateStoryDtoValidator;
        public StoryService(IStoryRepository storyRepository, IMapper mapper, IJwtTokenHandler jwtTokenHandler, BaseDtoValidator<PostStoryDto>postStoryDtoValidator, BaseDtoValidator<UpdateStoryDto>updateStoryDtoValidator)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
            _postStoryDtoValidator = postStoryDtoValidator;
            _updateStoryDtoValidator = updateStoryDtoValidator;
        }   

        public async Task<List<Story>> GetStoriesAsync(int pageNumber,int pageSize)
        {
            var stories = await _storyRepository.GetStoriesAsync(pageNumber,pageSize);
            return stories;
        }

        public async Task<Story> GetStoryByIdAsync(int Id)
        {
            var story =  await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null) throw new NotFoundException("Story not found");
            return story;
        }

        public async Task<Story> PostStoryAsync(PostStoryDto postStoryDto)
        {
            _postStoryDtoValidator.ValidateDTO(postStoryDto);
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != postStoryDto.AuthorName) throw new UnauthorizedException("You are not logged in");
            Story story = _mapper.Map<Story>(postStoryDto);
            story.CreatedAt = DateTime.UtcNow;
            story.UpdatedAt = DateTime.UtcNow;
            var newStory = await _storyRepository.PostStoryAsync(story);
            return newStory;
        }

        public async Task<Story> UpdateStoryAsync(int Id, UpdateStoryDto updateStoryDto)
        {
            _updateStoryDtoValidator.ValidateDTO(updateStoryDto);
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            Boolean auth = await CheckAuthor(loggedInUser, Id);
            if (!auth) throw new UnauthorizedException("You are not authorized to update this story");
            Story story = _mapper.Map<Story>(updateStoryDto);
            var updatedStory = await _storyRepository.UpdateStoryAsync(Id, story);
            return updatedStory;
        }
        public async Task<bool> DeleteStoryAsync(int Id)
        {
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            Boolean auth = await CheckAuthor(loggedInUser, Id);
            if (!auth) throw new UnauthorizedException("You are not authorized to delete this story");
            return await _storyRepository.DeleteStoryAsync(Id);
        }
        public async Task<List<Story>> GetSearchedStoriesAsync(int pageNumber, int pageSize, string pattern)
        {
            var stories = await _storyRepository.GetSearchedStoriesAsync(pageNumber, pageSize, pattern);
            return stories.ToList();
        }
        public async Task<List<Story>> GetStoriesOfAUserAsync(int pageNumber, int pageSize, string username)
        {
            var stories = await _storyRepository.GetStoriesOfAUserAsync(pageNumber, pageSize, username);
            return stories.ToList();
        }
        public async Task<int> CountStoriesAsync()
        {
            return await _storyRepository.CountStoriesAsync();
        }
        public async Task<int> CountSearchedStoriesAsync(string pattern)
        {
            return await _storyRepository.CountSearchedStoriesAsync(pattern);
        }
        public async Task<int> CountStoriesOfAUserAsync(string username)
        {
            return await _storyRepository.CountStoriesOfAUserAsync(username);
        }
        private async Task<Boolean> CheckAuthor(string loggedInUser, int Id)
        {
            if (loggedInUser == null) return false;
            var curstory = await _storyRepository.GetStoryByIdAsync(Id);
            if (curstory == null) return false;
            var authorName = ((Story)curstory).AuthorName;
            if (authorName != loggedInUser) return false;
            return true;
        }
    }
}
