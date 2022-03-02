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
    public class PostsRepositoryGetByTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;

        public PostsRepositoryGetByTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _commentRepository = new CommentRepository(_fixture.BlogDbContext);
            _postRepository = new PostRepository(fixture.BlogDbContext);
            _blogRepository = new BlogRepository(fixture.BlogDbContext);
        }

        [Fact(DisplayName = "PostRepository_GetByAsync_Success")]
        public async Task PostRepository_GetByAsync_Success()
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

            //Act
           var blogById = await _postRepository.GetByIdAsync(post.PostId);

            //Assert
            Assert.NotNull(blogById);
            Assert.Equal(newPost.PostName, blogById.PostName);
            Assert.Equal(newPost.Text, blogById.Text);
            Assert.Equal(newPost.BlogId, blogById.BlogId);
            Assert.Empty(newPost.Comment);
        }

        [Fact(DisplayName = "PostRepository_GetCommentsByPostIdAsync_Success")]
        public async Task PostRepository_GetCommentsByPostIdAsync_Success()
        {
            //Arrange
            var newBlog = BlogBuilder.Create().Build();
            var blog = await _blogRepository.AddAsync(newBlog);

            var newPost = PostBuilder.Create()
                .WithPostId(Guid.NewGuid())
                .WithName(Lorem.Words(5))
                .WithText(Lorem.Sentence(100))
                .WithBlogId(blog.BlogId)
                .WithComments(10)
                .Build();

            var post = await _postRepository.AddAsync(newPost);

            //Act
            var blogById = await _postRepository.GetByIdWithCommentsAsync(post.PostId);

            //Assert
            Assert.NotNull(blogById);
            Assert.Equal(newPost.PostName, blogById.PostName);
            Assert.Equal(newPost.Text, blogById.Text);
            Assert.Equal(newPost.BlogId, blogById.BlogId);
            Assert.NotEmpty(newPost.Comment);
        }
    }
}