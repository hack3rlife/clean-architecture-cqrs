using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Delete
{
    public class DeleteBlogRequestCommandValidator : AbstractValidator<DeleteBlogRequestCommand>
    {
        public DeleteBlogRequestCommandValidator()
        {
            RuleFor(x => x.BlogId).NotEqual(Guid.Empty).WithMessage("{PropertyName} cannot be empty");
        }
    }
}