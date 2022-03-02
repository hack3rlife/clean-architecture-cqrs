using FluentValidation;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Commands.Delete
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostRequestCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty().WithMessage("{PropertyName} cannot be empty.");
        }
    }
}
