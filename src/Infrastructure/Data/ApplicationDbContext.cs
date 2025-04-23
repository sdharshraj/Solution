using Microsoft.EntityFrameworkCore;
using Domain.Entities; // Assuming you're using domain entities

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
