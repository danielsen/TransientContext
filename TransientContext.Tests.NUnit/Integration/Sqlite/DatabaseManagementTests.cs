using NUnit.Framework;
using TransientContext.Tests.NUnit.Common.Data.Sqlite;

namespace TransientContext.Tests.NUnit.Integration.Sqlite
{
    [TestFixture]
    public class DatabaseManagementTests
    {
        [Test]
        public void should_create_database()
        {
            var fixture = new DatabaseFixture();

            var exists = fixture.TestDatabase.Exists();
            Assert.True(exists);
            
            fixture.Dispose();
        }

        [Test]
        public void should_drop_database()
        {
            var fixture = new DatabaseFixture();
            fixture.TestDatabase.Drop();

            var exists = fixture.TestDatabase.Exists();
            Assert.False(exists);
        }
    }
}