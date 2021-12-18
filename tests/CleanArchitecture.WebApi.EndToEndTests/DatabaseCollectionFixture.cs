using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests
{
    [CollectionDefinition("DatabaseCollectionFixture")]
    public class DatabaseCollectionFixture : ICollectionFixture<DataBaseFixture>
    {
    }
}
