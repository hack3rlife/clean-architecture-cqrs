using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IBlogRepository : IAsyncRepository<Blog>
    {
        Task<Blog> GetByIdWithPostsAsync(Guid blogId);
    }
}