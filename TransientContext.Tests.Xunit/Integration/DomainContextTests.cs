using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransientContext.Tests.Xunit.Common.Data;
using Xunit;

namespace TransientContext.Tests.Xunit.Integration
{
    [Collection("Database")]
    public class DomainContextTests
    {
        private readonly DomainContext _domainContext;

        public DomainContextTests(DatabaseFixture databaseFixture)
        {
            _domainContext = databaseFixture.DbContext;
        }

        [Fact]
        public async Task InsertUsers()
        {
            await _domainContext.Users.AddAsync(new User {Username = "Alice"});
            await _domainContext.Users.AddAsync(new User {Username = "Jane"});
            await _domainContext.SaveChangesAsync();

            var count = await _domainContext.Users.CountAsync();
            Assert.Equal(2, count);
        }
    }
}
