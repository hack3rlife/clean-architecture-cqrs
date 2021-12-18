using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Profiles;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Commands.Update
{
    public class UpdateBlogRequestCommandHandlerTests
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly UpdateBlogRequestCommandHandler _sut;

        public UpdateBlogRequestCommandHandlerTests()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });
            var mapper = configurationProvider.CreateMapper();

            _sut = new UpdateBlogRequestCommandHandler(_mockBlogRepository.Object, mapper);
        }

        [Fact(DisplayName = "Update_ExistingBlog_IsCalledOnce")]
        public async Task Update_ExistingBlog_IsCalledOnce()
        {
            // Arrange 
            var updateBlogCommand = new UpdateBlogRequestCommand
            {
                BlogId = Guid.NewGuid(),
                BlogName = "UpdateBlog"
            };

            _mockBlogRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(await Task.Run(() => new Blog()))
                .Verifiable();

            // Act
            var result = await _sut.Handle(updateBlogCommand, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Unit>();
            _mockBlogRepository.Verify(
                x => x.UpdateAsync(It.Is<Blog>(
                    b =>
                    b.BlogId == updateBlogCommand.BlogId
                    && b.BlogName == updateBlogCommand.BlogName)),
                Times.Once);
        }

        [Fact(DisplayName = "Update_BlogWhichDoesNotExist_ThrowsNotFoundException")]
        public async Task Update_BlogWhichDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var updateBlogCommand = new UpdateBlogRequestCommand()
            {
                BlogId = Guid.NewGuid(),
                BlogName = "UpdateBlogWhichDoesNotExistInDataBase"
            };

            // Act
            Func<Task> act = async () => await _sut.Handle(updateBlogCommand, new CancellationToken(false));

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"{nameof(Blog)} with guid ({updateBlogCommand.BlogId}) was not found");
        }
    }
}
