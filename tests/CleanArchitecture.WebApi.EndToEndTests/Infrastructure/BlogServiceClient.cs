using System.Collections.Generic;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    public class BlogServiceClient
    {
        private string _baseUrl = "/api/blogs";

        private readonly EndToEndClientFactory _endToEndClientFactory;
        private readonly EndToEndClient _endToEndClient;

        public BlogServiceClient(EndToEndClientFactory endToEndClientFactory)
        {
            _endToEndClientFactory = endToEndClientFactory;
            _endToEndClient = _endToEndClientFactory.Create();
        }

        public async Task<ApiResponse<List<GetBlogsQueryResponse>>> GetBlogs(GetBlogsRequestQuery getBlogsRequestQuery)
        {
            var url = $"{_baseUrl}?skip={getBlogsRequestQuery.skip}&take={getBlogsRequestQuery.take}";

            return await _endToEndClient.GetApiResponseAsync<List<GetBlogsQueryResponse>>(url);
        }
    }
}
