using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Delete
{
    public class DeleteBlogRequestCommand : IRequest
    {
        public Guid BlogId { get; set; }
    }

    public class DeleteBlogRequestCommandHandler : IRequestHandler<DeleteBlogRequestCommand>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public DeleteBlogRequestCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteBlogRequestCommand request, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<Blog>(request);

            var blogResult = await _blogRepository.GetByIdAsync(blog.BlogId);

            if (blogResult == null)
            {
                throw new NotFoundException(nameof(Blog), blog.BlogId);
            }

            await _blogRepository.DeleteAsync(blog);

            return Unit.Value;

        }
    }
}
