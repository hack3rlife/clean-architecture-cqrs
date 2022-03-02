using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll
{
    public class GetPostsQueryResponse
    {
        public Guid PostId { get; set; }
        public string PostName { get; set; }
        public Guid BlogId { get; set; }
    }
}
