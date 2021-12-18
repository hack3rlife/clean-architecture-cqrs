using AutoMapper;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Delete;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetAll;
using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            // In-coming
            CreateMap<AddBlogRequestCommand, Blog>();
            CreateMap<DeleteBlogRequestCommand, Blog>();
            CreateMap<UpdateBlogRequestCommand, Blog>();
            CreateMap<GetByBlogIdRequestQuery, Blog>();

            // Out-coming
            CreateMap<Blog, AddBlogCommandResponse>().ReverseMap();
            CreateMap<Blog, GetBlogsQueryResponse>().ReverseMap();
            CreateMap<Blog, GetByBlogIdQueryResponse>().ReverseMap();
        }
    }
}
