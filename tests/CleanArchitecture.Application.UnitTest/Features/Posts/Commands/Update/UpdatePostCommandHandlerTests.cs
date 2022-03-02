using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Update;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Update
{
    public class UpdatePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly UpdatePostCommandHandler _sut;

        public UpdatePostCommandHandlerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new UpdatePostCommandHandler(_mockPostRepository.Object, mapper);
        }

        [Fact(DisplayName = "Update_Post_Success")]
        public async Task Update_Post_Success()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var updatePostCommand = new UpdatePostRequestCommand
            {
                BlogId = Guid.NewGuid(),
                PostId = postId,
                PostName = "This how you unit test",
                Text = "Unit Testing Rocks"
            };

            _mockPostRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => new Post {PostId = postId}))
                .Verifiable();

            _mockPostRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Post>()))
                .Verifiable();

            // Act
            var result = await _sut.Handle(updatePostCommand, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Unit>();
            result.Should().Be(Unit.Value);

            _mockPostRepository
                .Verify(x => x.GetByIdAsync(It.Is<Guid>(p => p == postId)),
                    Times.Once);

            _mockPostRepository
                .Verify(x => x.UpdateAsync(
                        It.Is<Post>(p => p.BlogId == updatePostCommand.BlogId
                                         && p.PostId == updatePostCommand.PostId
                                         && p.PostName == updatePostCommand.PostName
                                         && p.Text == updatePostCommand.Text)),
                    Times.Once);
        }

        [Fact(DisplayName = "Update_PostWhichDoesNotExist_ThrowsNotFoundException")]
        public async Task Update_PostWhichDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var updatePostCommand = new UpdatePostRequestCommand
            {
                BlogId = Guid.NewGuid(),
                PostId = postId,
                PostName = "This how you unit test",
                Text = "Unit Testing Rocks"
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(updatePostCommand, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Post)} with guid ({updatePostCommand.PostId}) was not found");

            _mockPostRepository.Verify(x => x.UpdateAsync(It.Is<Post>(p => p.PostId == updatePostCommand.PostId)),
                Times.Never);
        }
    }
}