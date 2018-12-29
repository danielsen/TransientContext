using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TransientContext.Tests
{
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
}
