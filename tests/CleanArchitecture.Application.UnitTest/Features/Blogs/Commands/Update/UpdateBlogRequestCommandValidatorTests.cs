using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Update;
using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Commands.Update
{
    public class UpdateBlogRequestCommandValidatorTests
    {
        [Fact]
        public async Task UpdateBlog_WithNullOrEmptyBlogName_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogName", "Blog Name cannot be null"),
                new ValidationFailure("BlogName", "Blog Name cannot be empty"),
            };

            var validator = new UpdateBlogRequestCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new UpdateBlogRequestCommand{ BlogId = Guid.NewGuid()});

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogName").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
            validationResult.Errors[1].ErrorMessage.Should().Be(expectedErrors[1].ErrorMessage);
        }

        [Fact]
        public async Task UpdateBlog_WithNullOrEmptyBlogId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogId", "Blog Id cannot be empty")
            };

            var validator = new UpdateBlogRequestCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new UpdateBlogRequestCommand{ BlogName = "UpdateBlog"});

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }

        [Fact]
        public async Task UpdateBlog_WithoutRequiredValues_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogId", "Blog Id cannot be empty"),
                new ValidationFailure("BlogName", "Blog Name cannot be null"),
                new ValidationFailure("BlogName", "Blog Name cannot be empty"),
            };

            var validator = new UpdateBlogRequestCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new UpdateBlogRequestCommand());

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
            validationResult.Errors[0].PropertyName.Should().Be("BlogId");
            validationResult.Errors[1].ErrorMessage.Should().Be(expectedErrors[1].ErrorMessage);
            validationResult.Errors[1].PropertyName.Should().Be("BlogName");
            validationResult.Errors[2].ErrorMessage.Should().Be(expectedErrors[2].ErrorMessage);
            validationResult.Errors[2].PropertyName.Should().Be("BlogName");
        }
    }
}
