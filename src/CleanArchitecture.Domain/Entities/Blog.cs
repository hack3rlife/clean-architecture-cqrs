using System;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public Blog()
        {
            Post = new HashSet<Post>();
        }

        public Guid BlogId { get; set; }
        public string BlogName { get; set; }

        public ICollection<Post> Post { get; }
    }
}
