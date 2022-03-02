using CleanArchitecture.Application.Features.Posts.Handlers.Commands.Add;
using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Posts.Commands.Add
{
    public class AddPostCommandValidatorTests
    {
        private readonly List<ValidationFailure> _expectedErrors;

        public AddPostCommandValidatorTests()
        {
            _expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogId", "Blog Id cannot be empty."),
                new ValidationFailure("PostName", "Post Name cannot be null or empty."),
                new ValidationFailure("Text", "Text cannot be null or empty.")
            };
        }

        [Fact]
        public async Task AddPost_WithEmptyOrNullBlogId_ThrowsError()
        {
            // Arrange
            var addPostRequestCommand = new AddPostRequestCommand
            {
                PostName = "PostName",
                Text = "Some Random Text"
            };

            var validator = new AddPostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(addPostRequestCommand);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(_expectedErrors[0].ErrorMessage);
        }

        [Fact]
        public async Task AddPost_WithEmptyOrNullPostName_ThrowsError()
        {
            // Arrange
            var addPostRequestCommand = new AddPostRequestCommand
            {
                BlogId = Guid.NewGuid(),
                Text = "Some Random Text"
            };

            var validator = new AddPostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(addPostRequestCommand);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "PostName").Should().BeTrue();
            validationResult.Errors[1].ErrorMessage.Should().Be(_expectedErrors[1].ErrorMessage);
        }

        [Fact]
        public async Task AddPost_WithEmptyOrNullText_ThrowsError()
        {
            // Arrange
            var addPostRequestCommand = new AddPostRequestCommand
            {
                BlogId = Guid.NewGuid(),
                PostName = "PostName"
            };

            var validator = new AddPostCommandValidator();

            // Act
            var validationResult = await validator.ValidateAsync(addPostRequestCommand);

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "Text").Should().BeTrue();
            validationResult.Errors[1].ErrorMessage.Should().Be(_expectedErrors[2].ErrorMessage);
        }
    }
}
