using Cefalo.TechDaily.Database.Context;
using Cefalo.TechDaily.Database.Models;
using Cefalo.TechDaily.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Repository.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly DataContext _context;
        public StoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Story>> GetStoriesAsync(int pageNumber,int pageSize)
        {
            //return await _context.Stories.ToListAsync();
            return await _context.Stories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<Story?> GetStoriesAsync(int Id)
        {
            return await _context.Stories.FindAsync(Id);
        }

        public async Task<Story?> PostStoryAsync(Story story)
        {
            _context.Stories.Add(story);
            await _context.SaveChangesAsync();
            return story;
        }
        public async Task<Story?> UpdateStoryAsync(int Id, Story story)
        {
            var myStory = await _context.Stories.FindAsync(Id);
            if (myStory == null) return null;
            myStory.Title = story.Title;
            myStory.Description = story.Description;
            myStory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return myStory;
        }
        public async Task<Boolean> DeleteStoryAsync(int Id)
        {
            var story = await _context.Stories.FindAsync(Id);
            if(story == null) return false;
            _context.Stories.Remove((Story)story);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Story>> GetStoriesOfAUserAsync(int pageNumber, int pageSize, string username)
        {
            return await _context.Stories.Where(s => s.AuthorName == username).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<int> CountStoriesAsync()
        {
            return await _context.Stories.CountAsync(); 
        }

        public async Task<List<Story>> GetSearchedStoriesAsync(int pageNumber, int pageSize, string pattern)
        {
            return await _context.Stories.Where(s => s.AuthorName.Contains(pattern) || s.Title.Contains(pattern)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountSearchedStoriesAsync(string pattern)
        {
            return await _context.Stories.Where(s => s.AuthorName.Contains(pattern) || s.Title.Contains(pattern)).CountAsync();
        }
        public async Task<int> CountStoriesOfAUserAsync(string username)
        {
            return await _context.Stories.Where(s => s.AuthorName == username).CountAsync();
        }
    }
}
