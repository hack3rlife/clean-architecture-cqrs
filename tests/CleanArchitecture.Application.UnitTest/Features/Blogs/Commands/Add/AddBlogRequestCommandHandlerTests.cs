using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Commands.Add
{
    public class AddBlogRequestCommandHandlerTests
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly AddBlogRequestCommandHandler _sut;

        public AddBlogRequestCommandHandlerTests()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();
            _sut = new AddBlogRequestCommandHandler(_mockBlogRepository.Object, mapper);
        }

        [Fact]
        public async Task AddBlog_WithValidParameter_Success()
        {
            // Arrange
            var addBlogCommand = new AddBlogRequestCommand
            {
                BlogId = Guid.NewGuid(),
                BlogName = "AddBlogCommand"
            };

            _mockBlogRepository
                .Setup(x => x.AddAsync(It.IsAny<Blog>()))
                .ReturnsAsync(await Task.Run(() => new Blog()))
                .Verifiable();

            // Act
            var blogResult = await _sut.Handle(addBlogCommand, CancellationToken.None);

            // Assert
            blogResult.Should().NotBeNull();
            _mockBlogRepository.Verify(
                x => x.AddAsync(It.Is<Blog>(x => x.BlogId == addBlogCommand.BlogId
                                                 && x.BlogName == addBlogCommand.BlogName)),
                Times.Once);
        }
    }
}