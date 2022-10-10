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
        Task<List<Story>> GetStoriesAsync(int pageNumber,int pageSize);
        Task<Story> GetStoryByIdAsync(int Id);
        Task<Story> PostStoryAsync(PostStoryDto postStoryDto);
        Task<Story> UpdateStoryAsync(int Id, UpdateStoryDto updateStoryDto);
        Task<Boolean> DeleteStoryAsync(int Id);
        Task<List<Story>> GetSearchedStoriesAsync(int pageNumber, int pageSize, string pattern);
        Task<List<Story>> GetStoriesOfAUserAsync(int pageNumber, int pageSize, string username);
        Task<int> CountStoriesAsync();
        Task<int> CountSearchedStoriesAsync(string pattern);
        Task<int> CountStoriesOfAUserAsync(string username);

    }
}
