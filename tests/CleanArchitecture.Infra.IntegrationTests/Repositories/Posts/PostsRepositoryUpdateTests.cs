using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infra.IntegrationTests.Builders;
using CleanArchitecture.Infrastructure.Repositories;
using LoremNET;
using Xunit;

namespace CleanArchitecture.Infra.IntegrationTests.Repositories.Posts
{
    [Collection("DatabaseCollectionFixture")]
    public class PostsRepositoryUpdateTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;

        public PostsRepositoryUpdateTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _commentRepository = new CommentRepository(_fixture.BlogDbContext);
            _postRepository = new PostRepository(fixture.BlogDbContext);
            _blogRepository = new BlogRepository(fixture.BlogDbContext);
        }

        [Fact(DisplayName = "PostRepository_UpdateAsync_Success")]
        public async Task PostRepository_UpdateAsync_Success()
        {
            //Arrange
            var newBlog = BlogBuilder.Create().Build();
            var blog = await _blogRepository.AddAsync(newBlog);

            var newPost = PostBuilder.Create()
                .WithPostId(Guid.NewGuid())
                .WithName(Lorem.Words(5))
                .WithText(Lorem.Sentence(100))
                .WithBlogId(blog.BlogId)
                .Build();

            var post = await _postRepository.AddAsync(newPost);
            post.PostName = "UpdatePost";

            //Act
            await _postRepository.UpdateAsync(post);

            //Assert
            var updatedPost = await _postRepository.GetByIdAsync(post.PostId);

            Assert.NotNull(updatedPost);
            Assert.Equal("UpdatePost", updatedPost.PostName);
            Assert.Equal(newPost.Text, updatedPost.Text);
            Assert.Equal(newPost.BlogId, updatedPost.BlogId);
        }
    }
}