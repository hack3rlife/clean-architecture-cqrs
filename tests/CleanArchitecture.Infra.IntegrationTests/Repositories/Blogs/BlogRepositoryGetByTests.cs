using System;
using System.Threading.Tasks;
using CleanArchitecture.Infra.IntegrationTests.Builders;
using CleanArchitecture.Infrastructure.Repositories;
using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests.Repositories.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class BlogRepositoryGetByTests
    {
        private readonly BlogRepository _blogRepository;
        private readonly DatabaseFixture _fixture;

        public BlogRepositoryGetByTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _blogRepository = new BlogRepository(_fixture.BlogDbContext);
        }

        [Fact(DisplayName = "BlogRepository_ByIdAsync_ShouldSucceed")]
        public async Task BlogRepository_ByIdAsync_Succeed()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var blog = BlogBuilder.Create()
                .WithId(guid)
                .Build();

            await _blogRepository.AddAsync(blog);

            // Act
            var blogById = await _blogRepository.GetByIdAsync(guid);

            //Assert
            Assert.NotNull(blogById);
            Assert.Equal(guid, blogById.BlogId);
            Assert.Empty(blogById.Post);
        }

        [Fact(DisplayName = "BlogRepository_GetPostsByBlogId_Success")]
        public async Task BlogRepository_GetPostsByBlogId_Success()
        {
            //Arrange
            var expectedBlog = BlogBuilder.Create()
                .WithPosts(1)
                .Build();

            await _blogRepository.AddAsync(expectedBlog);

            //Act
            var blog = await _blogRepository.GetByIdWithPostsAsync(expectedBlog.BlogId);

            //Assert
            Assert.NotNull(blog);
            Assert.Equal(expectedBlog.BlogId, blog.BlogId);

        }
    }
}