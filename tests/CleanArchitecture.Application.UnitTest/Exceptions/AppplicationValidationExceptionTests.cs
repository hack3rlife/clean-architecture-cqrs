using CleanArchitecture.Application.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using System.Collections.Generic;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Exceptions
{
    public class AppplicationValidationExceptionTests
    {
        [Fact]
        public void SingleValidationFailureCreatesASingleElementErrorDictionary()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("name", "cannot be null"),
            };

            // Act
            var actual = new ApplicationValidationException(failures).Errors;

            // Assert
            actual.Keys.Should().NotBeNullOrEmpty();
            actual.Keys.Should().Contain("name");
            actual["name"].Should().BeEquivalentTo("cannot be null");
        }

        [Fact]
        public void MultipleValidationFailureCreatesMultiplesElementErrorDictionary()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("name", "cannot be null"),
                new ValidationFailure("name", "cannot be empty or blank"),
                new ValidationFailure("name", "cannot be longer than 255 characters"),
                new ValidationFailure("id", "cannot be empty guid"),
                new ValidationFailure("id", "cannot be null")
            };

            // Act
            var actual = new ApplicationValidationException(failures).Errors;

            // Assert
            actual.Keys.Should().NotBeNullOrEmpty();
            actual.Keys.Should().Contain("name", "id");
            actual["name"].Should().Contain(new[]
            {
                "cannot be null",
                "cannot be empty or blank",
                "cannot be longer than 255 characters",
            });

            actual["id"].Should().Contain(new[]
            {
                "cannot be null",
                "cannot be empty guid",
            });
        }
    }
}
