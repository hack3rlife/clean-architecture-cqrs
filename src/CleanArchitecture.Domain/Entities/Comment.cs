using System;

namespace CleanArchitecture.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid CommentId { get; set; }
        public string CommentName { get; set; }
        public string Email { get; set; }
        public Guid PostId { get; set; }
    }
}
