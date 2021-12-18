using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests
{
    [CollectionDefinition("DatabaseCollectionFixture")]
    public class DatabaseCollectionFixture : ICollectionFixture<DatabaseFixture>
    {
    }
}
