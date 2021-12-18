using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy
{
    public class GetByBlogIdQueryResponse
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Post> Post { get; } = new List<Post>();
    }
}