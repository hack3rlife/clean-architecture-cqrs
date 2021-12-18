using FluentValidation;

namespace CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add
{
    public class AddBlogRequestCommandValidator : AbstractValidator<AddBlogRequestCommand>
    {
        public AddBlogRequestCommandValidator()
        {
            RuleFor(x => x.BlogName).NotNull().WithMessage("{PropertyName} cannot be null");          
            RuleFor(x => x.BlogName).NotEmpty().WithMessage("{PropertyName} cannot be empty");
        }
    }
}
