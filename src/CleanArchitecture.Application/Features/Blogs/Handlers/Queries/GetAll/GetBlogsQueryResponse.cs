using System;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll
{
    public class GetBlogsQueryResponse
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
    }
}