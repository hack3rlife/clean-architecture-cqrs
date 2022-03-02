using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll;
using FluentAssertions;
using LoremNET;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Posts
{
    [Collection("DatabaseCollectionFixture")]
    public class GetPostsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public GetPostsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Get_Posts_Ok")]
        public async Task Get_Posts_Ok()
        {
            // Arrange
            var blogResponseMessage = await _client.PostAsync("/api/blogs/",
                new StringContent(JsonConvert.SerializeObject(new AddBlogRequestCommand { BlogName = "AddBlog" }),
                    Encoding.UTF8,
                    "application/json"));
            var blogContent = await blogResponseMessage.Content.ReadAsStringAsync();

            var blog = JsonConvert.DeserializeObject<AddBlogCommandResponse>(blogContent);

            var newPost = new AddPostRequestCommand
            {
                BlogId = blog.BlogId,
                PostName = "Get_Posts",
                Text = Lorem.Sentence(100)
            };

            var postResponseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));

            var post = JsonConvert.DeserializeObject<AddPostCommandResponse>(await postResponseMessage.Content.ReadAsStringAsync());

            // Act
            var responseMessage = await _client.GetAsync("/api/posts");
            var content = await responseMessage.Content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<List<GetPostsQueryResponse>>(content);

            // Assert
            posts.Should().BeOfType<List<GetPostsQueryResponse>>();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
