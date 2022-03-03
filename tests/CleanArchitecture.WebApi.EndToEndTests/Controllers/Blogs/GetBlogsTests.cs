using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll;
using CleanArchitecture.WebApi.EndToEndTests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class GetBlogsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly BlogServiceClient _blogServiceClient;

        public GetBlogsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _blogServiceClient = _factory.Services.GetRequiredService<BlogServiceClient>();
        }

        [Fact]
        public async Task Get_Blogs_Ok()
        {
            // Arrange
            var getBlogsRequestQuery = new GetBlogsRequestQuery();

            // Act
            var apiResponse = await _blogServiceClient.GetBlogs(getBlogsRequestQuery);

            // Assert
            apiResponse.Should().NotBeNull();
            apiResponse.Result.Should().BeOfType<List<GetBlogsQueryResponse>>();
            apiResponse.ProblemDetails.Should().BeNull();
            apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
