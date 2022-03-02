using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add
{
    public class AddPostRequestCommand : IRequest<AddPostCommandResponse>
    {
        public Guid PostId { get; set; }
        public string PostName { get; set; }
        public string Text { get; set; }
        public Guid BlogId { get; set; }
    }

    public class AddPostCommandHandler : IRequestHandler<AddPostRequestCommand, AddPostCommandResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public AddPostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<AddPostCommandResponse> Handle(AddPostRequestCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request);

            var postResult = await _postRepository.AddAsync(post);

            return _mapper.Map<AddPostCommandResponse>(postResult);
        }
    }
}
