using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class BlogRepository : EfRepository<Blog>, IBlogRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public BlogRepository(BlogDbContext blogDbContext) : base(blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Blog> GetByIdWithPostsAsync(Guid blogId)
        {
            return await _blogDbContext
                    .Blog
                    .Include(x => x.Post)
                    .FirstOrDefaultAsync(x => x.BlogId == blogId);
        }

        public override async Task<Blog> GetByIdAsync(Guid id)
        {
            return await _blogDbContext.Blog.AsNoTracking().FirstOrDefaultAsync(b=> b.BlogId == id);
        }
    }
}
