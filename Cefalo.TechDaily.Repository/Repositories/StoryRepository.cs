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

        public async Task<List<Story>> GetStories()
        {
            return await _context.Stories.ToListAsync();
        }
        public async Task<Story?> GetStoryById(int Id)
        {
            return await _context.Stories.FindAsync(Id);
        }

        public async Task<Story?> PostStory(Story story)
        {
            _context.Stories.Add(story);
            await _context.SaveChangesAsync();
            return story;
        }
        public async Task<Story?> UpdateStory(int Id, Story story)
        {
            var myStory = await _context.Stories.FindAsync(Id);
            if (myStory == null) return null;
            myStory.Title = story.Title;
            myStory.Description = story.Description;
            myStory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return myStory;
        }
        public async Task<Boolean> DeleteStory(int Id)
        {
            var story = await _context.Stories.FindAsync(Id);
            if(story == null) return false;
            _context.Stories.Remove((Story)story);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Story>> GetSearchedStories(string pattern)
        {
            return await _context.Stories.Where(s => s.AuthorName.Contains(pattern) || s.Title.Contains(pattern)).ToListAsync();
        }
        public async Task<List<Story>> GetStoriesOfAUser(string username)
        {
            return await _context.Stories.Where(s => s.AuthorName == username).ToListAsync();
        }
    }
}
