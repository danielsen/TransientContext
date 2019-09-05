using System;
using Microsoft.EntityFrameworkCore;
using TransientContext.Common;
using TransientContext.Sqlite;

namespace TransientContext.Tests.NUnit.Common.Data.Sqlite
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            TestDatabase = new TestDatabaseBuilder()
                .WithConnectionString("Data Source=domain.db")
                .Build();
            TestDatabase.Create();

            var builder = new DbContextOptionsBuilder<DomainContext>()
                .UseSqlite(TestDatabase.ConnectionString);
            DbContext = new DomainContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }
        
        public ITestDatabase TestDatabase { get; }
        public DomainContext DbContext { get;  }
        
        public void Dispose()
        {
            TestDatabase.Drop();
        }
    }
}