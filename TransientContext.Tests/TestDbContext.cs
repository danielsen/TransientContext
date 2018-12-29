using System;
using Microsoft.EntityFrameworkCore;

namespace TransientContext.Tests
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
