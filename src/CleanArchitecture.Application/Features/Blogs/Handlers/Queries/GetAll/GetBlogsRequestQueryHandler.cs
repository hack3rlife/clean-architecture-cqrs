using AutoMapper;
using CleanArchitecture.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll
{
    public class GetBlogsRequestQuery : IRequest<IEnumerable<GetBlogsQueryResponse>>
    {
        public int skip { get; }
        public int take { get; }

        /// <inheritdoc />
        public GetBlogsRequestQuery()
        {
            skip = 0;
            take = 100;
        }

        public GetBlogsRequestQuery(int skip, int take)
        {
            this.skip = skip;
            this.take = take;
        }
    }

    public class GetBlogsRequestQueryHandler : IRequestHandler<GetBlogsRequestQuery, IEnumerable<GetBlogsQueryResponse>>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetBlogsRequestQueryHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetBlogsQueryResponse>> Handle(GetBlogsRequestQuery request, CancellationToken cancellationToken)
        {
            var skip = request.skip < 0 ? 0 : request.skip;
            var take = request.take <= 0 ? 10 : request.take;

            var blogsResult = await _blogRepository.ListAllAsync(skip, take);

            return _mapper.Map<List<GetBlogsQueryResponse>>(blogsResult);
        }
    }
}