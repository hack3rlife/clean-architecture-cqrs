using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Queries.GetBy
{
    public class GetByPostIdQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly GetByPostIdQueryHandler _sut;

        public GetByPostIdQueryHandlerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new GetByPostIdQueryHandler(_mockPostRepository.Object, mapper);
        }

        [Fact(DisplayName = "GetBy_PostId_Success")]
        public async Task GetBy_PostId_Success()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var postName = "PostName";
            var text = "Some Random Text";

            var today = DateTime.Today;

            var getByPostIdQuery = new GetByPostIdQuery
            {
                PostId = postId
            };

            var postByQuery = new Post
            {
                PostId = postId,
                PostName = postName,
                Text = text,
                BlogId = Guid.NewGuid(),
                Comment =
                {
                    new Comment
                    {
                        PostId = postId,
                        CommentId = Guid.NewGuid()
                    }
                },
                CreatedBy = "admin",
                CreatedDate = today,
                LastUpdate = today,
                UpdatedBy = "admin",

            };

            _mockPostRepository.Setup(x => x.GetByIdWithCommentsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => postByQuery))
                .Verifiable();

            // Act
            var post = await _sut.Handle(getByPostIdQuery, CancellationToken.None);

            // Assert
            post.Should().NotBeNull();
            post.Should().BeOfType<GetByPostIdQueryResponse>();
            post.BlogId.Should().Be(postByQuery.BlogId);
            post.PostId.Should().Be(postByQuery.PostId);
            post.PostName.Should().Be(postByQuery.PostName);
            post.Text.Should().Be(postByQuery.Text);
            post.CreatedBy.Should().Be("admin");
            post.CreatedDate.Should().Be(today);
            post.UpdatedBy.Should().Be("admin");
            post.LastUpdate.Should().Be(today);
            post.Comment.Should().NotBeNull();

            _mockPostRepository.Verify(x => x.GetByIdWithCommentsAsync(getByPostIdQuery.PostId),
                Times.Once);
        }

        [Fact(DisplayName = "GetBy_PostId_Fail")]
        public async Task GetBy_PostId_Fail()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var getByPostIdQuery = new GetByPostIdQuery
            {
                PostId = postId
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(getByPostIdQuery, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Post)} with guid ({getByPostIdQuery.PostId}) was not found");

            _mockPostRepository.Verify(x => x.UpdateAsync(It.Is<Post>(p => p.PostId == getByPostIdQuery.PostId)),
                Times.Never);
        }
    }
}