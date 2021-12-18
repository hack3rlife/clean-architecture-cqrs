using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Queries.GetBy
{
    public class GetByBlogIdRequestQueryHandlerTests
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly GetByBlogIdRequestQueryHandler _sut;

        public GetByBlogIdRequestQueryHandlerTests()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();
            _sut = new GetByBlogIdRequestQueryHandler(_mockBlogRepository.Object, mapper);
        }

        [Fact(DisplayName = "GetBy_BlogId_Success")]
        public async Task GetBy_BlogId_Success()
        {
            // Arrange
            var _blogId = Guid.NewGuid();
            var _blogName = "GetBy";
            var today = DateTime.Today;

            var getByBlogQuery = new GetByBlogIdRequestQuery
            {
                BlogId = _blogId
            };

            var getByBlog = new Blog
            {
                BlogId = _blogId,
                BlogName = _blogName,
                CreatedBy = "admin",
                CreatedDate = today,
                LastUpdate = today,
                UpdatedBy = "admin",
                Post = { new Post
                {
                    BlogId = _blogId,
                    PostId = Guid.NewGuid(),
                    PostName = "PostForGetByBlogId"
                } }
            };

            _mockBlogRepository.Setup(x => x.GetByIdWithPostsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => getByBlog))
                .Verifiable();

            // Act
            var blog = await _sut.Handle(getByBlogQuery, new CancellationToken(false));

            // Assert
            blog.Should().NotBeNull("because we are expecting an existing blog and returns null");
            blog.BlogId.Should().Be(_blogId, $"because we assigned {_blogId}");
            blog.BlogName.Should().Be(_blogName, $"because we assigned value {_blogName}");
            blog.CreatedBy.Should().Be("admin");
            blog.CreatedDate.Should().Be(today);
            blog.UpdatedBy.Should().Be("admin");
            blog.LastUpdate.Should().Be(today);
            _mockBlogRepository.Verify(x => x.GetByIdWithPostsAsync(_blogId), Times.Once);
        }

        [Fact(DisplayName = "GetBy_BlogId_Fail")]
        public async Task GetBy_BlogId_Fail()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var getByBlogQuery = new GetByBlogIdRequestQuery
            {
                BlogId = guid
            };

            // Act
           Func<Task> act = async () => await _sut.Handle(getByBlogQuery, new CancellationToken(false));

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Blog)} with guid ({guid}) was not found");

            _mockBlogRepository.Verify(x => x.GetByIdWithPostsAsync(guid), Times.Once);
        }
    }
}