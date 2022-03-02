using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using FluentAssertions;
using LoremNET;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Posts
{
    [Collection("DatabaseCollectionFixture")]
    public class DeletePostsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public DeletePostsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Delete_Post_Ok")]
        public async Task Delete_Post_Ok()
        {
            // Arrange
            var blogResponseMessage = await _client.PostAsync("/api/blogs/",
                new StringContent(JsonConvert.SerializeObject(new AddBlogRequestCommand { BlogName = "Delete_Post_Ok" }),
                    Encoding.UTF8,
                    "application/json"));
            var blogContent = await blogResponseMessage.Content.ReadAsStringAsync();

            var blog = JsonConvert.DeserializeObject<AddBlogCommandResponse>(blogContent);

            var newPost = new AddPostRequestCommand
            {
                BlogId = blog.BlogId,
                PostName = "Delete_Post",
                Text = Lorem.Sentence(100)
            };

            var postResponseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));

            var post = JsonConvert.DeserializeObject<AddPostCommandResponse>(await postResponseMessage.Content.ReadAsStringAsync());

            // Act
           var responseMessage = await _client.DeleteAsync($"/api/posts/{post.PostId}");
           var result = JsonConvert.DeserializeObject(await responseMessage.Content.ReadAsStringAsync());

            // Assert
            result.Should().BeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
