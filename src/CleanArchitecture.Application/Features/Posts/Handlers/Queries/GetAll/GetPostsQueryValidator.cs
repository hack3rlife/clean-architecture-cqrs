using FluentValidation;

namespace CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(x => x.Take).LessThanOrEqualTo(100).WithMessage("{PropertyName} cannot be greater than hundred.");
        }
    }
}
