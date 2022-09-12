using Cefalo.TechDaily.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.TechDaily.Database.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
