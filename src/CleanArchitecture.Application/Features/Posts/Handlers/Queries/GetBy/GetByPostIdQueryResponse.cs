using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy
{
    public class GetByPostIdQueryResponse
    {
        public Guid PostId { get; set; }
        public string PostName { get; set; }
        public string Text { get; set; }
        public Guid BlogId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UpdatedBy { get; set; }

        public ICollection<Comment> Comment { get; set; } = new List<Comment>();
    }
}
