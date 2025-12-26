using BookStoreRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
namespace BookStoreRest.Controllers
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options) {
            Database.EnsureCreated();
        }
    }
}
