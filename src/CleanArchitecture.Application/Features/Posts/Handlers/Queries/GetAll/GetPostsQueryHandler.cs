using AutoMapper;
using CleanArchitecture.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll
{
    public class GetPostsQuery : IRequest<IEnumerable<GetPostsQueryResponse>>
    {
        public int Skip { get; }
        public int Take { get; }

        public GetPostsQuery()
        {
             Skip = 0;
             Take = 10;
        }

        public GetPostsQuery(int skip = 0, int take = 10)
        {
            this.Skip = skip;
            this.Take = take;
        }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<GetPostsQueryResponse>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPostsQueryResponse>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var skip = request.Skip < 0 ? 0 : request.Skip;
            var take = request.Take <= 0 ? 10 : request.Take;

            var posts = await _postRepository.ListAllAsync(skip, take);

             return _mapper.Map<IEnumerable<GetPostsQueryResponse>>(posts);
        }
    }
}
