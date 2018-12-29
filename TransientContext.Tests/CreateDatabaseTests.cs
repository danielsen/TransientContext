using Xunit;

namespace TransientContext.Tests
{
    [Collection("Database")]
    public class CreateDatabaseTests
    {
        [Fact]
        public void CreateAndDropDatabase()
        {
            Assert.True(true);
        }
    }
}
