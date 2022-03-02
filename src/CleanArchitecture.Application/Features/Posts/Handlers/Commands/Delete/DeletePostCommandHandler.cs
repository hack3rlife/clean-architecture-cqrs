using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Commands.Delete
{
    public class DeletePostRequestCommand : IRequest
    {
        public Guid PostId { get; set; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostRequestCommand>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public DeletePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeletePostRequestCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request);

            var postResult = await _postRepository.GetByIdAsync(post.PostId);

            if (postResult == null)
            {
                throw new NotFoundException(nameof(Post), post.PostId);
            }
                
            await _postRepository.DeleteAsync(postResult);

            return Unit.Value;
        }
    }
}