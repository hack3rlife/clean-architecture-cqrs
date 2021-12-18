using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Blogs
{
    [Collection("DatabaseCollectionFixture")]
    public class UpdateBlogsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UpdateBlogsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Update_Blog_Success")]
        public async Task Update_Blog_Success()
        {
            // Arrange
            var newBlog = new Blog
            {
                BlogName = LoremNET.Lorem.Words(20)
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(newBlog), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/blogs/", content);
            var blogCommandResponse = JsonConvert.DeserializeObject<AddBlogCommandResponse>(await response.Content.ReadAsStringAsync());

            var httpResponseMessage = await _client.GetAsync($"api/blogs/{blogCommandResponse.BlogId}");
            var blogToBeUpdated = JsonConvert.DeserializeObject<GetByBlogIdQueryResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            blogToBeUpdated.BlogName = "Updated Blog";

            var httpContent = new StringContent(JsonConvert.SerializeObject(blogToBeUpdated), Encoding.UTF8, "application/json");

            // Act
            var responseMessage = await _client.PutAsync("/api/blogs/", httpContent);

            // Assert
            responseMessage.Should().NotBeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Update_BlogWhichDoesNotExist_ManagesNotFoundException")]
        public async Task Update_BlogWhichDoesNotExist_ManagesNotFoundException()
        {
            // Arrange
            var newBlog = new Blog
            {
                BlogId = Guid.NewGuid(),
                BlogName = LoremNET.Lorem.Words(20)
            };

            var content = new StringContent(JsonConvert.SerializeObject(newBlog), Encoding.UTF8, "application/json");

            // Act
            var responseMessage = await _client.PutAsync("/api/blogs/", content);
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await responseMessage.Content.ReadAsStringAsync());

            // Assert
            responseMessage.Should().NotBeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Should().NotBeNull();
        }
    }
}
