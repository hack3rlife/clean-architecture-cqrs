using System.Threading.Tasks;
using CleanArchitecture.Infra.IntegrationTests.Builders;
using CleanArchitecture.Infrastructure.Repositories;
using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests.Repositories.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class BlogRepositoryListAllTests
    {
        private readonly BlogRepository _blogRepository;
        private readonly DatabaseFixture _fixture;

        public BlogRepositoryListAllTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _blogRepository = new BlogRepository(_fixture.BlogDbContext);
        }

        [Fact(DisplayName = "BlogRepository_ListAllAsync_ShouldReturnAllBlogs")]
        public async Task BlogRepository_ListAllAsync_Succeed()
        {
            //Arrange
            for (var i = 0; i < 10; i++)
            {
                var blog = BlogBuilder.Create().Build();
                await _blogRepository.AddAsync(blog);
            }

            var skip = 1;
            var take = 5;

            //Act
            var blogs = await _blogRepository.ListAllAsync(skip, take);

            //Assert
            Assert.NotNull(blogs);
            Assert.True(blogs.Count == take);
        }
    }
}