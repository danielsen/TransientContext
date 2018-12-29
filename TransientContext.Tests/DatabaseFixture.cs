using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransientContext.Postgresql;
using Xunit;

namespace TransientContext.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            TestDatabase = new TestDatabaseBuilder()
                .WithConfiguration(configuration)
                .Build();
            TestDatabase.Create();

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseNpgsql(TestDatabase.ConnectionString);
            DbContext = new TestDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }


        public ITestDatabase TestDatabase { get; }

        public TestDbContext DbContext { get; }

        public void Dispose()
        {
            TestDatabase.Drop();
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture>
    {
    }
}
