using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Delete;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Commands.Delete
{
    public class DeleteBlogRequestCommandHandlerTests
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly DeleteBlogRequestCommandHandler _sut;

        public DeleteBlogRequestCommandHandlerTests()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new DeleteBlogRequestCommandHandler(_mockBlogRepository.Object, mapper);
        }

        [Fact(DisplayName = "Delete_ExistingBlog_Success")]
        public async Task Delete_ExistingBlog_Success()
        {
            // Arrange
            var deleteBlogCommand = new DeleteBlogRequestCommand
            {
                BlogId = Guid.NewGuid(),
            };

            _mockBlogRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => new Blog { BlogId = deleteBlogCommand.BlogId }))
                .Verifiable();

            _mockBlogRepository.Setup(x => x.DeleteAsync(It.IsAny<Blog>())).Verifiable();

            // Act
            var result = await _sut.Handle(deleteBlogCommand, new CancellationToken(false));

            // Assert
            result.Should().BeOfType<Unit>();

            _mockBlogRepository.Verify(
                x => x.DeleteAsync(It.Is<Blog>(b => b.BlogId == deleteBlogCommand.BlogId)),
                Times.Once);
        }

        [Fact(DisplayName = "Delete_BlogWhichDoesNotExist_ThrowsNotFoundException")]
        public async Task Delete_BlogWhichDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var deleteBlogCommand = new DeleteBlogRequestCommand
            {
                BlogId = Guid.NewGuid(),
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(deleteBlogCommand, new CancellationToken(false));

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Blog)} with guid ({deleteBlogCommand.BlogId}) was not found");
        }
    }
}
