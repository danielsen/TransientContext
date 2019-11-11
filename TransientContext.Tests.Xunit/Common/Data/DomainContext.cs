using Microsoft.EntityFrameworkCore;

namespace TransientContext.Tests.Xunit.Common.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
