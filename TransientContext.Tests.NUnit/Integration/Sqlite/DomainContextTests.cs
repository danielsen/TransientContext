using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TransientContext.Tests.NUnit.Common.Data;
using TransientContext.Tests.NUnit.Common.Data.Sqlite;

namespace TransientContext.Tests.NUnit.Integration.Sqlite
{
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

        [Test]
        public async Task should_delete_entity()
        {
            var user = await AddEntity();
            var entity = await _domainContext.Set<User>()
                .FindAsync(user.Id);

            _domainContext.Set<User>().Remove(entity);
            await _domainContext.SaveChangesAsync();

            var count = await _domainContext.Users.CountAsync();
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task should_modify_entity()
        {
            var user = await AddEntity();
            var entity = await _domainContext.Set<User>()
                .FindAsync(user.Id);

            entity.Username = "Bob";
            var updated = _domainContext.Set<User>()
                .Update(entity);
            await _domainContext.SaveChangesAsync();
            
            Assert.AreEqual("Bob", updated.Entity.Username);
        }
    }
}