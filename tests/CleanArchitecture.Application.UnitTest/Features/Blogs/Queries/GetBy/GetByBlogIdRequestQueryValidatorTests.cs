using CleanArchitecture.Application.Features.Blogs.Handlers.Queries.GetBy;
using FluentAssertions;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Features.Blogs.Queries.GetBy
{
    public class GetByBlogIdRequestQueryValidatorTests
    {
        [Fact]
        public async Task DeleteBlog_UsingInvalidBlogId_ThrowsError()
        {
            // Arrange
            var expectedErrors = new List<ValidationFailure>
            {
                new ValidationFailure("BlogId", "Blog Id cannot be empty")
            };

            var validator = new GetByBlogRequestQueryValidator();

            // Act
            var validationResult = await validator.ValidateAsync(new GetByBlogIdRequestQuery());

            // Assert
            validationResult.Errors.Should().NotBeNullOrEmpty();
            validationResult.Errors.TrueForAll(x => x.PropertyName == "BlogId").Should().BeTrue();
            validationResult.Errors[0].ErrorMessage.Should().Be(expectedErrors[0].ErrorMessage);
        }
    }
}
