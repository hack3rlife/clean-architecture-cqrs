using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update
{
    public class UpdateBlogRequestCommand : IRequest
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
    }

    public class UpdateBlogRequestCommandHandler : IRequestHandler<UpdateBlogRequestCommand>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public UpdateBlogRequestCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateBlogRequestCommand request, CancellationToken cancellationToken)
        {
            var blogResult = await _blogRepository.GetByIdAsync(request.BlogId);

            if (blogResult == null)
                throw new NotFoundException(nameof(Blog), request.BlogId);

            var blogToBeUpdated = _mapper.Map<Blog>(request);

            await _blogRepository.UpdateAsync(blogToBeUpdated);

            return Unit.Value;
        }
    }
}
