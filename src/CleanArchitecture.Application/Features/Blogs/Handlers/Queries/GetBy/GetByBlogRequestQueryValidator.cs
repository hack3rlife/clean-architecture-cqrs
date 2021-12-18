using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy
{
    public class GetByBlogRequestQueryValidator : AbstractValidator<GetByBlogIdRequestQuery>
    {
        public GetByBlogRequestQueryValidator()
        {
            RuleFor(x => x.BlogId).NotEqual(Guid.Empty).WithMessage("{PropertyName} cannot be empty");
        }
    }
}
