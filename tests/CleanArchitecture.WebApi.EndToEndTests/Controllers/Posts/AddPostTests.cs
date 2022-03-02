﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using LoremNET;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Posts
{
    [Collection("DatabaseCollectionFixture")]
    public class AddCommentsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public AddCommentsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Add_Post_Ok")]
        public async Task Add_Post_Ok()
        {
            // Arrange
            var blogResponseMessage = await _client.PostAsync("/api/blogs/",
                new StringContent(JsonConvert.SerializeObject(new Blog { BlogName = "AddBlog" }),
                    Encoding.UTF8,
                    "application/json"));
            var blogContent = await blogResponseMessage.Content.ReadAsStringAsync();

            var blog = JsonConvert.DeserializeObject<Blog>(blogContent);

            var newPost = new Post
            {
                BlogId = blog.BlogId,
                PostName = "AddPost",
                Text = Lorem.Sentence(100)
            };

            // Act
            var responseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));
            var content = await responseMessage.Content.ReadAsStringAsync();

            var post = JsonConvert.DeserializeObject<Post>(content);

            // Assert
            post.Should().BeOfType<Post>();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "AddPost_WithEmptyBlogId_ThrowsAnError")]
        public async Task AddPost_WithEmptyBlogId_ThrowsAnError()
        {
            // Arrange
           var newPost = new Post
            {
                BlogId = Guid.Empty,
                PostName = "AddPost",
                Text = Lorem.Sentence(100)
            };

            // Act 
            var responseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));
            var content = await responseMessage.Content.ReadAsStringAsync();

            var post = JsonConvert.DeserializeObject<Post>(content);

            // Assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            post.Should().BeOfType<Post>();

        }

        [Fact(DisplayName = "AddPost_WithEmptyName_ThrowsAnError")]
        public async Task AddPost_WithEmptyName_ThrowsAnError()
        {
            // Arrange
            var newPost = new Post
            {
                BlogId = Guid.NewGuid(),
                PostName = "",
                Text = Lorem.Sentence(100)
            };

            // Act 
            var responseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));
            var content = await responseMessage.Content.ReadAsStringAsync();

            var post = JsonConvert.DeserializeObject<Post>(content);

            // Assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            post.Should().BeOfType<Post>();
        }
    }
}
