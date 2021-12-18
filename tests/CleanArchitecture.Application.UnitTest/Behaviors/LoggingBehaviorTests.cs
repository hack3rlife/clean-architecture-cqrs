using CleanArchitecture.Application.Features.Blogs.Handlers.Commands.Add;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Behaviors;
using Xunit;

namespace CleanArchitecture.Application.UnitTest.Behaviours
{
    public class LoggingBehaviorTests
    {
        private readonly Mock<ILogger<LoggingBehavior<AddBlogRequestCommand, AddBlogCommandResponse>>> _logger;

        public LoggingBehaviorTests()
        {
            _logger = new Mock<ILogger<LoggingBehavior<AddBlogRequestCommand, AddBlogCommandResponse>>>();
        }

        [Fact]
        public async Task Logging_LogInformation_Success()
        {
            // Arrange
            var addBlogRequestCommand = new AddBlogRequestCommand
            {
                BlogId = Guid.NewGuid(),
                BlogName = "AddBlogCommand"
            };

            var logBehaviour = new LoggingBehavior<AddBlogRequestCommand, AddBlogCommandResponse>(_logger.Object);

            // Act
            var response = await logBehaviour.Handle(addBlogRequestCommand,
                CancellationToken.None, () => Task.FromResult(new AddBlogCommandResponse {BlogId = Guid.NewGuid()}));

            // Assert
            response.Should().NotBeNull();
            response.BlogId.Should().NotBeEmpty();
        }
    }
}