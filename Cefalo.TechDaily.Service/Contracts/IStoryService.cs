using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Service.Contracts
{
    public interface IStoryService
    {
        Task<List<Story>> GetStories();
        Task<Story> GetStoryById(int Id);
        Task<Story> PostStory(PostStoryDto postStoryDto);
        Task<Story> UpdateStory(int Id, UpdateStoryDto updateStoryDto);
        Task<Boolean> DeleteStory(int Id);
        Task<List<Story>> GetSearchedStories(string pattern);
        Task<List<Story>> GetStoriesOfAUser(string username);

    }
}
