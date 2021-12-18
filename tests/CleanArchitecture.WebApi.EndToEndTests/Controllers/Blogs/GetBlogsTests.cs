using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class GetBlogsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public GetBlogsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Blogs_Ok()
        {
            // Arrange

            // Act
            var responseMessage = await _client.GetAsync("/api/blogs/");
            var content = await responseMessage.Content.ReadAsStringAsync();

            var blogs = JsonConvert.DeserializeObject<List<Blog>>(content);

            // Assert
            blogs.Should().BeOfType<List<Blog>>();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
