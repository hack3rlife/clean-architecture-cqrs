using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy
{
    public class GetByBlogIdRequestQuery : IRequest<GetByBlogIdQueryResponse>
    {
        public Guid BlogId { get; set; }
    }

    public class GetByBlogIdRequestQueryHandler : IRequestHandler<GetByBlogIdRequestQuery, GetByBlogIdQueryResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetByBlogIdRequestQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<GetByBlogIdQueryResponse> Handle(GetByBlogIdRequestQuery request, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<Blog>(request);

            var blogResult = await _blogRepository.GetByIdWithPostsAsync(blog.BlogId);
            
            if (blogResult == null)
                throw new NotFoundException(nameof(Blog), blog.BlogId);

            return _mapper.Map<GetByBlogIdQueryResponse>(blogResult);
        }
    }
}
