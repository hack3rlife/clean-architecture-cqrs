using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Commands.Add
{
    public class AddBlogRequestCommandValidatorTests
    {
        [Fact]
        public async Task AddBlog_WithEmptyOrNullBlogName_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogName", "Blog Name cannot be null"),
                new ValidationFailure("BlogName", "Blog Name cannot be empty"),
            };

            var validator = new AddBlogRequestCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new AddBlogRequestCommand());

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogName").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
            validationResult.Errors[1].ErrorMessage.Should().Be(expectedErrors[1].ErrorMessage);
        }
    }
}
