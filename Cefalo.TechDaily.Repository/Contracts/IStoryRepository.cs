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
        Task<List<Story>> GetStoriesAsync(int pageNumber,int pageSize);
        Task<Story?> GetStoryByIdAsync(int Id);
        Task<Story?> PostStoryAsync(Story story);
        Task<Story?> UpdateStoryAsync(int Id, Story story);
        Task<Boolean> DeleteStoryAsync(int Id);
        Task<List<Story>> GetSearchedStoriesAsync(int pageNumber, int pageSize, string pattern);
        Task<List<Story>> GetStoriesOfAUserAsync(int pageNumber, int pageSize, string username);
        Task<int> CountStoriesAsync();
        Task<int> CountSearchedStoriesAsync(string pattern);
        Task<int> CountStoriesOfAUserAsync(string username);
    }
}
