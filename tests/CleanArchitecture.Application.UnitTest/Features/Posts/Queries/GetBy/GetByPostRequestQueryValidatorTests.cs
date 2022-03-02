using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetAll;
using FluentAssertions;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Queries.GetBy
{
    public class GetByPostRequestQueryValidatorTests
    {
        [Fact]
        public async Task DeletePost_UsingInvalidPostId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Take", "Take cannot be greater than hundred.")
            };

            var validator = new GetPostsQueryValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new GetPostsQuery(0,101));

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "Take").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }
    }
}
