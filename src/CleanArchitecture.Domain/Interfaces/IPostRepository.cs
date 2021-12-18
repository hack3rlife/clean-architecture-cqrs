using System;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IPostRepository: IAsyncRepository<Post>
    {
        Task<Post> GetByIdWithCommentsAsync(Guid postId);
    }
}
