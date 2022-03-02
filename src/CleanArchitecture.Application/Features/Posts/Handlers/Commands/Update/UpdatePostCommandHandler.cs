using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Commands.Update
{
    public class UpdatePostRequestCommand : IRequest
    {
        public Guid PostId { get; set; }
        public string PostName { get; set; }
        public string Text { get; set; }
        public Guid BlogId { get; set; }
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostRequestCommand>
    {
        private IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdatePostRequestCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request);

            var postResult = await _postRepository.GetByIdAsync(post.PostId);

            if (postResult == null)
            {
                throw new NotFoundException(nameof(Post), post.PostId);
            }

            //TODO: Find a way to update this mapping using AutoMapper. So far, is failing when using DBContext
            postResult.PostName = post.PostName;
            postResult.Text = post.Text;
            postResult.BlogId = post.BlogId;
            postResult.UpdatedBy = post.UpdatedBy;
            postResult.LastUpdate = post.LastUpdate;

            await _postRepository.UpdateAsync(postResult);

            return Unit.Value;
            
        }
    }
}
