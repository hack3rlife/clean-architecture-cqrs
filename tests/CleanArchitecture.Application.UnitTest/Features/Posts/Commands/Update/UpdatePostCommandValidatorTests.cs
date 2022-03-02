using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Update;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Update
{
    public class UpdatePostCommandValidatorTests
    {
        [Fact]
        public async Task UpdatePost_WithNullOrEmptyPostName_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("PostName", "Post Name cannot be null or empty."),
            };

            var updatePostRequest = new UpdatePostRequestCommand
            {
                Text = "How to Unit Test the right way",
                BlogId = Guid.NewGuid()
            };

            var validator = new UpdatePostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(updatePostRequest);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "PostName").Should().BeTrue();
            validationResult.Errors[1].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }

        [Fact]
        public async Task UpdatePost_WithNullOrEmptyText_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Text", "Text cannot be null or empty."),
            };

            var updatePostRequest = new UpdatePostRequestCommand
            {
                PostName = "PostName",
                BlogId = Guid.NewGuid()
            };

            var validator = new UpdatePostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(updatePostRequest);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "Text").Should().BeTrue();
            validationResult.Errors[1].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }

        [Fact]
        public async Task UpdatePost_WithNullOrEmptyBlogId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogId", "Blog Id cannot be empty."),
            };

            var updatePostRequest = new UpdatePostRequestCommand
            {
                PostName = "PostName",
                Text = "How to Unit Test the right way",
            };

            var validator = new UpdatePostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(updatePostRequest);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }
    }
}
