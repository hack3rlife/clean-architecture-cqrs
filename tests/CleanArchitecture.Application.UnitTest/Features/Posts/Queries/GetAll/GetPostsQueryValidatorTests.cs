using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Posts.Handlers.Queries.GetBy;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Queries.GetAll
{
   public class GetPostsQueryValidatorTests
    {
        [Fact]
        public async Task DeletePost_UsingInvalidPostId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("PostId", "Post Id cannot be empty.")
            };

            var validator = new GetByPostIdQueryValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new GetByPostIdQuery());

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "PostId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }
    }
}
