using Microsoft.EntityFrameworkCore;
using TodoAPI.Models.Entities;

namespace TodoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Todo> todos { get; set; }  
    }
}
