using Cefalo.TechDaily.Database.Configurations;
using Cefalo.TechDaily.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.TechDaily.Database.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new UserConfiguration().Configure(builder.Entity<User>());
            new StoryConfiguration().Configure(builder.Entity<Story>());
        }
    }
}
