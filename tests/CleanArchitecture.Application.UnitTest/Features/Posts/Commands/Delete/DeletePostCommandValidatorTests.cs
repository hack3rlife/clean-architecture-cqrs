using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Delete;
using FluentAssertions;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Delete
{
    public class DeletePostCommandValidatorTests
    {
        [Fact]
        public async Task DeletePost_UsingInvalidPostId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("PostId", "Post Id cannot be empty.")
            };

            var validator = new DeletePostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new DeletePostRequestCommand());

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "PostId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }
    }
}
