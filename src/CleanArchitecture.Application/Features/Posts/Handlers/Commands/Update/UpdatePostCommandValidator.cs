using FluentValidation;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Commands.Update
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostRequestCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.BlogId).NotEmpty().WithMessage("{PropertyName} cannot be empty.");
            RuleFor(x => x.PostName).NotNull().NotEmpty().WithMessage("{PropertyName} cannot be null or empty.");
            RuleFor(x => x.Text).NotNull().NotEmpty().WithMessage("{PropertyName} cannot be null or empty.");
        }
    }
}
