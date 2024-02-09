using BlogApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        //Dbset
        public DbSet<Post> Posts { get; set; }
    }
}
