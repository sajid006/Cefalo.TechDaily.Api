using Cefalo.TechDaily.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.Contracts
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetStories();
        Task<Story?> GetStoryById(int Id);
        Task<Story?> PostStory(Story story);
        Task<Story?> UpdateStory(int Id, Story story);
        Task<Boolean> DeleteStory(int Id);
        Task<List<Story>> GetSearchedStories(string pattern);
        Task<List<Story>> GetStoriesOfAUser(string username);
    }
}
