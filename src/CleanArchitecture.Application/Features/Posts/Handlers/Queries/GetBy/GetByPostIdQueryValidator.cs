using FluentValidation;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy
{
    public class GetByPostIdQueryValidator : AbstractValidator<GetByPostIdQuery>
    {
        public GetByPostIdQueryValidator()
        {
            RuleFor(x => x.PostId).NotEmpty().WithMessage("{PropertyName} cannot be empty.");
        }
    }
}
