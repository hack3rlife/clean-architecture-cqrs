using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add
{
    public class AddBlogRequestCommand : IRequest<AddBlogCommandResponse>
    {
        public Guid BlogId { get; set; }
        public string BlogName { get; set; }
    }

    public class AddBlogRequestCommandHandler : IRequestHandler<AddBlogRequestCommand, AddBlogCommandResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public AddBlogRequestCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<AddBlogCommandResponse> Handle(AddBlogRequestCommand request, CancellationToken cancellationToken)
        {
            var blog = _mapper.Map<Blog>(request);

            var blogResult = await _blogRepository.AddAsync(blog);

            return _mapper.Map<AddBlogCommandResponse>(blogResult);
        }
    }
}