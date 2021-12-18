using CleanArchitecture.Application.Behaviors;
using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using FluentAssertions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Behaviors
{
    public class ValidatorBehaviorTests
    {

        [Fact]
        public async Task ValidationBehavior_WhenRequiredIsCorrect_DoesNoThrowApplicationException()
        {
            // Arrange
            var validations = new List<IValidator<AddBlogRequestCommand>>
            {
                new AddBlogRequestCommandValidator()
            };

            var validation = new ValidatorBehavior<AddBlogRequestCommand, AddBlogCommandResponse>(validations);

            // Act

            Func<Task> action = async () => await validation.Handle(new AddBlogRequestCommand{BlogName = "AddBlog"}, CancellationToken.None, () => Task.FromResult(new AddBlogCommandResponse { BlogId = Guid.NewGuid() }));

            // Assert
            await action.Should().NotThrowAsync<ApplicationException>();
        }

        [Fact]
        public async Task ValidationBehavior_WhenRequiredDataIsMissingOrIncorrect_ThrowsApplicationException()
        {
            // Arrange
            var validations = new List<IValidator<AddBlogRequestCommand>>
            {
                new AddBlogRequestCommandValidator()
            };

            var validation = new ValidatorBehavior<AddBlogRequestCommand, AddBlogCommandResponse>(validations);

            // Act

            Func<Task> action = async () => await validation.Handle(new AddBlogRequestCommand(), CancellationToken.None, () => Task.FromResult(new AddBlogCommandResponse { BlogId = Guid.NewGuid() }));

            // Assert
            await action.Should().ThrowAsync<ApplicationException>();

        }
    }
}