using System.Threading.Tasks;
using CleanArchitecture.Infra.IntegrationTests.Builders;
using CleanArchitecture.Infrastructure.Repositories;
using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests.Repositories.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class BlogRepositoryAddTests
    {
        private readonly BlogRepository _blogRepository;
        private readonly DatabaseFixture _fixture;

        public BlogRepositoryAddTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _blogRepository = new BlogRepository(_fixture.BlogDbContext);
        }

        [Fact(DisplayName = "BlogRepository_AddBlog_Success")]
        public async Task BlogRepository_AddAsync_Success()
        {
            //Arrange
            var newBlog = BlogBuilder.Create().Build();

            //Act
            var blog = await _blogRepository.AddAsync(newBlog);

            //Assert
            Assert.NotNull(blog);
            Assert.Equal(newBlog.BlogId, blog.BlogId);
            Assert.Equal(newBlog.BlogName, blog.BlogName);
        }
    }
}