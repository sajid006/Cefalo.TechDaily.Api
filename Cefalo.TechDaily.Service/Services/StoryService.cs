
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

namespace Cefalo.TechDaily.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public StoryService(IStoryRepository storyRepository, IMapper mapper, IJwtTokenHandler jwtTokenHandler)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
        }   

        public async Task<List<Story>> GetStories()
        {
            var stories = await _storyRepository.GetStories();
            return stories.ToList();
        }

        public async Task<Story> GetStoryById(int Id)
        {
            var story =  await _storyRepository.GetStoryById(Id);
            if (story == null) throw new NotFoundException("Story not found");
            return story;
        }

        public async Task<Story> PostStory(PostStoryDto postStoryDto)
        {
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            if (loggedInUser != postStoryDto.AuthorName) throw new UnauthorizedException("You are not logged in");
                Story story = _mapper.Map<Story>(postStoryDto);
            story.CreatedAt = DateTime.UtcNow;
            story.UpdatedAt = DateTime.UtcNow;
            var newStory = await _storyRepository.PostStory(story);
            if (newStory == null) throw new BadRequestException("Cannot post story");
            return newStory;
        }

        public async Task<Story> UpdateStory(int Id, UpdateStoryDto updateStoryDto)
        {
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            Boolean Auth = await CheckAuthor(loggedInUser, Id);
            if (!Auth) throw new UnauthorizedException("You are not authorized to update this story");
            if (Id != updateStoryDto.Id) throw new UnauthorizedException("You are not authorized to update this story");
            Story story = _mapper.Map<Story>(updateStoryDto);
            var newStory = await _storyRepository.UpdateStory(Id, story);
            if (newStory == null) throw new BadRequestException("Cannot update story");
            return newStory;
        }
        public async Task<bool> DeleteStory(int Id)
        {
            var loggedInUser = _jwtTokenHandler.GetLoggedinUsername();
            Boolean Auth = await CheckAuthor(loggedInUser, Id);
            if (!Auth) throw new UnauthorizedException("You are not authorized to update this story");
            return await _storyRepository.DeleteStory(Id);
        }
        private async Task<Boolean> CheckAuthor(string loggedInUser, int Id)
        {
            if (loggedInUser == null) return false;
            var curstory = await GetStoryById(Id);
            if (curstory == null) return false;
            var authorName = ((Story)curstory).AuthorName;
            if (authorName != loggedInUser) return false;
            return true;
        }
    }
}
