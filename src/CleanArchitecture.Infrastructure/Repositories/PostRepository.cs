using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class PostRepository : EfRepository<Post>, IPostRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public PostRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Post> GetByIdWithCommentsAsync(Guid postId)
        {
            return await _blogDbContext
                .Post
                .Include(x => x.Comment)
                .FirstOrDefaultAsync(x => x.PostId == postId);
        }
    }
}
