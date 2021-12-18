﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Blogs
{

    [Collection("DatabaseCollectionFixture")]
    public class DeleteBlogsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public DeleteBlogsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Delete_Blog_Success")]
        public async Task Delete_Blog_Success()
        {
            // Arrange
            var newBlog = new Blog
            {
                BlogId = Guid.NewGuid(),
                BlogName = LoremNET.Lorem.Words(20)
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(newBlog), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/blogs/", httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var blog = JsonConvert.DeserializeObject<Blog>(responseContent);

            // Act
            var responseMessage = await _client.DeleteAsync($"/api/blogs/{blog.BlogId}");

            // Assert
            responseMessage.Should().NotBeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Delete_BlogWhichDoesNotExist_ManagesNotFoundException")]
        public async Task Delete_BlogWhichDoesNotExist_ManagesNotFoundException()
        {
            // Arrange
            var blogId = Guid.NewGuid();

            // Act
            var responseMessage = await _client.DeleteAsync($"/api/blogs/{blogId}");
            var result = JsonConvert.DeserializeObject<ProblemDetails>(await responseMessage.Content.ReadAsStringAsync());

            // Assert
            responseMessage.Should().NotBeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Should().NotBeNull();
        }

    }
}
