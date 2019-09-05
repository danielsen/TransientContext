# TransientContext

`TransientContext` is a simple tool for creating transient or disposable databases for integration
testing. During integration testing `TransientContext` can be used to create a disposable database
from an existing schema and drop the database after tests are complete. `TransientContext` supports
PostgreSQL and SQLite databases.

### Packages

- Current Version: `1.1.0`
- Target Framework: `.NET Standard 2.0`

### Dependencies

- [Npgsql](https://www.nuget.org/packages/Npgsql)
- [System.Data.SQLite](https://www.nuget.org/packages/System.Data.SQLite/)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/2.2.0)

### Development Dependencies

- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.2.0)
- [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/2.2.6)
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/2.2.0)
- [Microsoft.Extensions.Configuration.UserSecrets](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets/2.2.0)
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/15.9.0)
- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/2.2.0)
- [Xunit](https://www.nuget.org/packages/xunit/)
- [Xunit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio/)
- [NUnit](https://www.nuget.org/packages/NUnit/)
- [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/)

### Examples and Usage

The examples below are taken directly from `TransientContext.Tests.NUnit` and `TransientContext.Tests.Xunit`. In
the PostgreSQL examples connection strings are stored in user secrets files as described in the [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=linux).

### NUnit Usage

1. Begin by creating a database fixture. In the example below, `DomainContext` represents
the `EntityFramework` DBContext that is being created. For reference, see `TransientContext.Tests.NUnit/Common/Data/DomainContext.cs`.

	    public class DatabaseFixture : IDisposable
	    {
	        public DatabaseFixture()
	        {
	            var configuration = new ConfigurationBuilder()
	                .AddUserSecrets("your-secret-guid")
	                .Build();
	
	            TestDatabase = new TestDatabaseBuilder()
	                .WithConfiguration(configuration)
	                .Build();
				
				/* Or for SQLite 
				 * 
            	 * TestDatabase = new TestDatabaseBuilder()
                 *     .WithConnectionString("Data Source=domain.db")
                 *      .Build();
                 */

	            TestDatabase.Create();

	            var builder = new DbContextOptionsBuilder<DomainContext>()
	                .UseNpgsql(TestDatabase.ConnectionString);
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

2. Add the `DatabaseFixture` as part of the `SetUp` and `TearDown` methods in your test fixtures.

	    [TestFixture]
	    public class DomainContextTests
	    {
	        private DatabaseFixture _fixture;
	        private DomainContext _domainContext;

	        [SetUp]
	        public void Setup()
	        {
	            _fixture = new DatabaseFixture();
	            _domainContext = _fixture.DbContext;
	        }

	        [TearDown]
	        public void Teardown()
	        {
	            _fixture.Dispose();
	        }

	        private async Task<User> AddEntity()
	        {
	            var entity = await _domainContext.Users.AddAsync(new User
	            {
	                Username = "Alice"
	            });
	            await _domainContext.SaveChangesAsync();
	            return entity.Entity;
	        }

	        [Test]
	        public async Task should_add_entity()
	        {
	            var user = await AddEntity();

	            var count = await _domainContext.Users.CountAsync();
	            Assert.NotNull(user.Id);
	            Assert.AreEqual(1, count);
	        }
		}

### XUnit Usage

The examples below assume XUnit as the testing framework. These examples are taken directly
from `TransientContext.Tests.Xunit`.

1. Begin by creating a database fixture. In the example below, `DomainContext` represents
the `EntityFramework` DBContext that is being created. For reference, see `TransientContext.Tests.NUnit/Common/Data/DomainContext.cs`.

	    public class DatabaseFixture : IDisposable
	    {
	        public DatabaseFixture()
	        {
	            var configuration = new ConfigurationBuilder()
	               .AddUserSecrets("your-secret-guid")
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

2. Add the `DatabaseFixture` as part of a test class constructor.

	    [Collection("Database")]
		public class UserTests
		{
			private readonly TestDbContext _testDbContext;

			public UserTests(DatabaseFixture databaseFixture)
			{
				_testDbContext = databaseFixture.DbContext;
			}

			[Fact]
			public async Task InsertUsers()
			{
				await _testDbContext.Users.AddAsync(new User {Username = "Alice"});
				await _testDbContext.Users.AddAsync(new User {Username = "Jane"});
				await _testDbContext.SaveChangesAsync();

				var count = await _testDbContext.Users.CountAsync();
				Assert.Equal(2, count);
			}
		}
