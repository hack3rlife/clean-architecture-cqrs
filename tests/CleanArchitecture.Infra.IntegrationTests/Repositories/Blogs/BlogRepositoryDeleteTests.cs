using System.Threading.Tasks;
using CleanArchitecture.Infra.IntegrationTests.Builders;
using CleanArchitecture.Infrastructure.Repositories;
using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests.Repositories.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class BlogRepositoryDeleteTests
    {
        private readonly BlogRepository _blogRepository;
        private readonly DatabaseFixture _fixture;

        public BlogRepositoryDeleteTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _blogRepository = new BlogRepository(_fixture.BlogDbContext);
        }

        [Fact(DisplayName = "BlogRepository_DeleteAsync_Success")]
        public async Task BlogRepository_DeleteAsync_Success()
        {
            //Arrange
            var newBlog = BlogBuilder.Create().Build();

            var blog = await _blogRepository.AddAsync(newBlog);

            //Act
            await _blogRepository.DeleteAsync(blog);

            //Assert
            var deletedBlog = await _blogRepository.GetByIdAsync(blog.BlogId);

            Assert.Null(deletedBlog);
        }
    }
}