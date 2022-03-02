using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Add
{
    public class AddPostCommandHandlerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly AddPostCommandHandler _sut;

        public AddPostCommandHandlerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new AddPostCommandHandler(_mockPostRepository.Object, mapper);
        }

        [Fact(DisplayName = "AddPost_WithRequiredValues_IsCalledOnce")]
        public async Task AddPost_WithRequiredValues_IsCalledOnce()
        {
            // Arrange
            var postId = Guid.NewGuid();

            var addPostCommand = new AddPostRequestCommand
            {
                PostName = "AddPost",
                Text = "How to Unit Test the right way",
                BlogId = Guid.NewGuid()
            };

            _mockPostRepository.
                Setup(x => x.AddAsync(It.IsAny<Post>()))
                .ReturnsAsync(await Task.Run(() => new Post{ PostId = postId}))
                .Verifiable();

            // Act 
            var post = await _sut.Handle(addPostCommand, CancellationToken.None);

            // Assert
            post.Should().BeOfType<AddPostCommandResponse>();
            post.PostId.Should().Be(postId);

            _mockPostRepository.Verify(x => x.AddAsync(
                    It.Is<Post>(p => p.PostName == addPostCommand.PostName
                                     && p.Text == addPostCommand.Text
                                     && p.BlogId == addPostCommand.BlogId)),
                Times.Once);
        }
    }
}
