using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy;
using FluentAssertions;
using LoremNET;
using Newtonsoft.Json;
using Xunit;

namespace CleanArchitecture.WebApi.EndToEndTests.Controllers.Posts
{
    [Collection("DatabaseCollectionFixture")]
    public class UpdatePostsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public UpdatePostsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact(DisplayName = "Update_Post_Ok")]
        public async Task Update_Post_Ok()
        {
            // Arrange
            var blogResponseMessage = await _client.PostAsync("/api/blogs/",
                new StringContent(JsonConvert.SerializeObject(new AddBlogRequestCommand { BlogName = "Update_Post_Ok" }),
                    Encoding.UTF8,
                    "application/json"));
            var blogContent = await blogResponseMessage.Content.ReadAsStringAsync();

            var blog = JsonConvert.DeserializeObject<AddBlogCommandResponse>(blogContent);

            var newPost = new AddPostRequestCommand
            {
                BlogId = blog.BlogId,
                PostName = "Update_Post",
                Text = Lorem.Sentence(100)
            };

            var postResponseMessage = await _client.PostAsync("/api/posts/",
                new StringContent(JsonConvert.SerializeObject(newPost),
                    Encoding.UTF8,
                    "application/json"));

            var post = JsonConvert.DeserializeObject<AddPostCommandResponse>(await postResponseMessage.Content.ReadAsStringAsync());

            var httpResponseMessage = await _client.GetAsync($"api/posts/{post.PostId}");
            var postToBeUpdated = JsonConvert.DeserializeObject<GetByPostIdQueryResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            postToBeUpdated.Text = Lorem.Paragraph(50, 100);
            
            // Act
           var responseMessage = await _client.PutAsync("/api/posts/",
               new StringContent(JsonConvert.SerializeObject(postToBeUpdated),
                   Encoding.UTF8,
                   "application/json"));
           var result = JsonConvert.DeserializeObject(await responseMessage.Content.ReadAsStringAsync());

            // Assert
            result.Should().BeNull();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
