using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Delete;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Delete
{
    public class DeletePostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly DeletePostCommandHandler _sut;

        public DeletePostCommandHandlerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new DeletePostCommandHandler(_mockPostRepository.Object, mapper);
        }

        [Fact(DisplayName = "Delete_ExistingPost_IsCalledOnce")]
        public async Task Delete_ExistingPost_IsCalledOnce()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var deletePostCommand = new DeletePostRequestCommand
            {
                PostId = postId
            };

            _mockPostRepository
                .Setup(x=> x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => new Post{PostId = postId}))
                .Verifiable();

            _mockPostRepository
                .Setup(x => x.DeleteAsync(It.IsAny<Post>()))
                .Verifiable();

            // Act
            var result =await _sut.Handle(deletePostCommand, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Unit>();
            result.Should().Be(Unit.Value);

            _mockPostRepository.Verify(x=> x.GetByIdAsync(It.Is<Guid>(p=> p == postId)), 
                Times.Once);

            _mockPostRepository.Verify(x => x.DeleteAsync(It.Is<Post>(p => p.PostId == deletePostCommand.PostId)),
                Times.Once);
        }

        [Fact(DisplayName = "Delete_PostWhichDoesNotExist_ThrowsNotFoundException")]
        public async Task Delete_PostWhichDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var deletePostCommand = new DeletePostRequestCommand
            {
                PostId = postId
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(deletePostCommand, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Post)} with guid ({deletePostCommand.PostId}) was not found");

            _mockPostRepository.Verify(x => x.DeleteAsync(It.Is<Post>(p => p.PostId == deletePostCommand.PostId)),
                Times.Never);
        }
    }
}