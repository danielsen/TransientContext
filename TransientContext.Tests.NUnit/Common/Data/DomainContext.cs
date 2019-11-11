using System;
using Microsoft.EntityFrameworkCore;

namespace TransientContext.Tests.NUnit.Common.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }

    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        
        public DbSet<User> Users { get; set; }
    }
}