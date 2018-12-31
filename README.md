# TransientContext

`TransientContext` is a simple tool for creating transient or disposable databases for integration
testing. During integration testing `TransientContext` can be used to create a disposable database
from an existing schema and drop the database after tests are complete.

### Packages

- Current Version: `0.1.0`
- Target Framework: `.NET Standard 2.0`

### Dependencies

- [Npgsql](https://www.nuget.org/packages/Npgsql)
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/2.2.0)

### Development Dependencies

- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/2.2.0)
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/2.2.0)
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/15.9.0)
- [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/2.2.0)
- [Xunit](https://www.nuget.org/packages/xunit/)
- [Xunit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio/)

### Usage

The examples below assume Xunit as the testing framework.

1. Begin by creating a database fixture. In the example below, `ApplicationDbContext` represents
the `EntityFramework` DBContext that is being created.

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

				var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
				builder.UseNpgsql(TestDatabase.ConnectionString);
				DbContext = new ApplicationDbContext(builder.Options);
				DbContext.Database.EnsureCreated();
			}


			public ITestDatabase TestDatabase { get; }

			public TestDbContext DbContext { get; }

			public void Dispose()
			{
				TestDatabase.Drop();
			}
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

### Caveats

As noted previously, this package was developed for use with XUnit tests but can be adapted
to work with other test frameworks.

This is a very basic package, contributions are welcome.
