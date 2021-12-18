using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class CommentRepository : EfRepository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
        }
    }
}
