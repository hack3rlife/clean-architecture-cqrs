using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy
{
    public class GetByPostIdQuery : IRequest<GetByPostIdQueryResponse>
    {
        public Guid PostId { get; set; }
    }

    public class GetByPostIdQueryHandler : IRequestHandler<GetByPostIdQuery, GetByPostIdQueryResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetByPostIdQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<GetByPostIdQueryResponse> Handle(GetByPostIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdWithCommentsAsync(request.PostId);

            if (post == null)
            {
                throw new NotFoundException(nameof(Post), request.PostId);
            }

            return _mapper.Map<GetByPostIdQueryResponse>(post);
        }
    }
}
