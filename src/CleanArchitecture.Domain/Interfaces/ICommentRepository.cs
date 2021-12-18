using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
    }
}