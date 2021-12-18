using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update
{
    public class UpdateBlogRequestCommandValidator : AbstractValidator<UpdateBlogRequestCommand>
    {
        public UpdateBlogRequestCommandValidator()
        {
            RuleFor(x => x.BlogId).NotEqual(Guid.Empty).WithMessage("{PropertyName} cannot be empty");
            RuleFor(x => x.BlogName).NotNull().WithMessage("{PropertyName} cannot be null");
            RuleFor(x => x.BlogName).NotEmpty().WithMessage("{PropertyName} cannot be empty");
        }
    }
}