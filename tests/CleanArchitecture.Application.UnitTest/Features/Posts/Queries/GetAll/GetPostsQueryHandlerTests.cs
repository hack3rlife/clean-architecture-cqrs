using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Queries.GetAll
{
    public class GetPostsQueryHandlerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly GetPostsQueryHandler _sut;

        public GetPostsQueryHandlerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new GetPostsQueryHandler(_mockPostRepository.Object, mapper);
        }

        [Fact(DisplayName = "GetBlogs_WithDefaultPagination_IsCalledOnce")]
        public async Task GetBlogs_WithDefaultPagination_IsCalledOnce()
        {
            // Arrange
            var getPostQuery = new GetPostsQuery();
            _mockPostRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Post>()))
                .Verifiable();

            // Act 
            var blogs = await _sut.Handle(getPostQuery, CancellationToken.None);

            // Assert
            blogs.Should().BeOfType<List<GetPostsQueryResponse>>();
            
            _mockPostRepository.Verify(x => x.ListAllAsync(0, 10), 
                Times.Once);
        }

        [Theory(DisplayName = "GetPostQuery_WithInvalidSkip_UsesDefaultSkipValue")]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        public async Task GetPostQuery_WithInvalidSkip_UsesDefaultSkipValue(int actualSkip, int expectedSkip)
        {
            // Arrange
            const int take = 10;

            var getPostQuery = new GetPostsQuery(actualSkip, take);
            _mockPostRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Post>()))
                .Verifiable();

            // Act 
            var blogs = await _sut.Handle(getPostQuery, CancellationToken.None);

            // Assert
            blogs.Should().BeOfType<List<GetPostsQueryResponse>>();

            _mockPostRepository.Verify(x => x.ListAllAsync(expectedSkip, take),
                Times.Once);
        }

        [Theory(DisplayName = "GetPostsQuery_WithInvalidTake_UsesDefaultSkipValue")]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task GetPostsQuery_WithInvalidTake_UsesDefaultSkipValue(int actualTake, int expectedTake)
        {
            // Arrange
            const int skip = 0;

            var getPostQuery = new GetPostsQuery(skip, actualTake);
            _mockPostRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Post>()))
                .Verifiable();

            // Act 
            var blogs = await _sut.Handle(getPostQuery, CancellationToken.None);

            // Assert
            blogs.Should().BeOfType<List<GetPostsQueryResponse>>();

            _mockPostRepository.Verify(x => x.ListAllAsync(skip, expectedTake),
                Times.Once);
        }
    }
}