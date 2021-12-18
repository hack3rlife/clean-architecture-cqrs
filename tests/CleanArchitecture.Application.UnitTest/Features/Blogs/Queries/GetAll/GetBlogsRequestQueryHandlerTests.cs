using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll;
using CleanArchitecture.Application.Profiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Queries.GetAll
{
    public class GetBlogsRequestQueryHandlerTests
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly GetBlogsRequestQueryHandler _sut;

        public GetBlogsRequestQueryHandlerTests()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<ProfileMapper>(); });

            var mapper = configurationProvider.CreateMapper();

            _sut = new GetBlogsRequestQueryHandler(_mockBlogRepository.Object, mapper);
        }

        [Fact(DisplayName = "GetBlogsQuery_GetAll_Success")]
        public async Task GetBlogsQuery_GetAll_Success()
        {
            const int skip = 1;
            const int take = 2;

            //Arrange
            _mockBlogRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Blog>()))
                .Verifiable();

            var query = new GetBlogsRequestQuery(skip, take);

            //Act
            var blogs = await _sut.Handle(query, new CancellationToken(false));

            //Assert
            blogs.Should().BeOfType<List<GetBlogsQueryResponse>>();

            _mockBlogRepository.Verify(x => x.ListAllAsync(skip, take), Times.Once);
        }

        [Theory(DisplayName = "GetBlogsQuery_WithInvalidSkip_UsesDefaultSkipValue")]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        public async Task GetBlogsQuery_WithInvalidSkip_UsesDefaultSkipValue(int actualSkip, int expectedSkip)
        {
            const int take = 10;

            //Arrange
            _mockBlogRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Blog>()))
                .Verifiable();

            var query = new GetBlogsRequestQuery(actualSkip, take);

            //Act
            var blogs = await _sut.Handle(query, new CancellationToken(false));

            //Assert
            blogs.Should().BeOfType<List<GetBlogsQueryResponse>>();

            _mockBlogRepository.Verify(x => x.ListAllAsync(expectedSkip, take), Times.Once);
        }

        [Theory(DisplayName = "GetBlogsQuery_WithInvalidSkip_UsesDefaultSkipValue")]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task GetBlogsQuery_WithInvalidTake_UsesDefaultSkipValue(int actualTake, int expectedTake)
        {
            const int skip = 0;

            //Arrange
            _mockBlogRepository.Setup(x => x.ListAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(await Task.Run(() => new List<Blog>()))
                .Verifiable();

            var query = new GetBlogsRequestQuery(skip, actualTake);

            //Act
            var blogs = await _sut.Handle(query, new CancellationToken(false));

            //Assert
            blogs.Should().BeOfType<List<GetBlogsQueryResponse>>();

            _mockBlogRepository.Verify(x => x.ListAllAsync(skip, expectedTake), Times.Once);
        }
    }
}