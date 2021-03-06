﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransientContext.Common;
using TransientContext.Postgresql;
using Xunit;

namespace TransientContext.Tests.Xunit.Common.Data
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
               .AddUserSecrets("57a04ea5-19b4-483f-af4d-a96930e166e5")
                .Build();
            TestDatabase = new TestDatabaseBuilder()
                .WithConfiguration(configuration)
                .Build();
            TestDatabase.Create();

            var builder = new DbContextOptionsBuilder<DomainContext>();
            builder.UseNpgsql(TestDatabase.ConnectionString);
            DbContext = new DomainContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }


        public ITestDatabase TestDatabase { get; }

        public DomainContext DbContext { get; }

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
