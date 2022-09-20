
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

namespace Cefalo.TechDaily.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        public StoryService(IStoryRepository storyRepository, IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
        }   

        public async Task<List<Story>> GetStories()
        {
            var stories = await _storyRepository.GetStories();
            return stories.ToList();
        }

        public async Task<Story> GetStoryById(int Id)
        {
            var story =  await _storyRepository.GetStoryById(Id);
            return story;
        }

        public async Task<Story> PostStory(PostStoryDto postStoryDto)
        {
            Story story = _mapper.Map<Story>(postStoryDto);
            story.CreatedAt = DateTime.Now;
            story.UpdatedAt = DateTime.Now;
            var newUser = await _storyRepository.PostStory(story);
            return newUser;
        }

        public async Task<Story> UpdateStory(int Id, UpdateStoryDto updateStoryDto)
        {
            if (Id != updateStoryDto.Id) return null;
            Story story = _mapper.Map<Story>(updateStoryDto);
            var newStory = await _storyRepository.UpdateStory(Id, story);
            return newStory;
        }
        public Task<bool> DeleteStory(int Id)
        {
            return _storyRepository.DeleteStory(Id);
        }
    }
}
